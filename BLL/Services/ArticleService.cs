using BLL.Models.Dtos;
using DAL.Repositories;
using DAL.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Services
{
	public class ArticleService : IArticleService
	{
		private readonly IArticleRepository _articleRepository;
		private readonly IUserRepository _userRepository;

		public ArticleService(IArticleRepository articleRepository,
			IUserRepository userRepository)
		{
			_articleRepository = articleRepository;
			_userRepository = userRepository;
		}

		/// <inheritdoc/>
		public async Task<ArticleDto> GetArticleByIdAsync(int id)
		{
			var article = await _articleRepository.GetArticleByIdAsync(id);
			if (article == null)
			{
				return null;
			}

			var articleDto = new ArticleDto
			{
				Author = new AuthorDto
				{
					FirstName = article.Author.FirstName,
					LastName = article.Author.LastName
				},
				Id = article.Id,
				Content = article.Content,
				Description = article.Description,
				PublishedAt = article.PublishedAt,
				Title = article.Title
			};
			return articleDto;
		}

		/// <inheritdoc/>
		public async Task<ArticlesDto> GetNewestArticlesAsync(int pageSize, int page)
		{
			var articlesCount = await _articleRepository.GetArticlesCountAsync();
			var totalPages = (int)Math.Ceiling(articlesCount / (double)pageSize);

			if (page > totalPages)
			{
				page = totalPages;
			}

			var articles = await _articleRepository.GetNewestArticlesAsync(pageSize, (page - 1) * pageSize);
			var articleDtos = articles.Select(x => new ArticleDto
			{
				Author = new AuthorDto
				{
					FirstName = x.Author.FirstName,
					LastName = x.Author.LastName
				},
				Id = x.Id,
				Content = x.Content,
				Description = x.Description,
				PublishedAt = x.PublishedAt,
				Title = x.Title
			}).ToArray();

			var articlesDto = new ArticlesDto
			{
				Articles = articleDtos,
				Count = articlesCount,
				PageSize = pageSize,
				CurrentPage = page,
				TotalPages = totalPages
			};

			return articlesDto;
		}

		/// <inheritdoc/>
		public async Task<ArticleDto> InsertAsync(ArticleDto articleModel, string userId)
		{
			var user = await _userRepository.GetUserByIdAsync(userId);
			if (user == null)
			{
				throw new ArgumentException("Invalid userId");
			}

			var article = new Article
			{
				Author = user,
				Content = articleModel.Content,
				Description = articleModel.Description,
				PublishedAt = DateTime.UtcNow,
				Title = articleModel.Title
			};

			article = await _articleRepository.InsertAsync(article);
			var articleDto = new ArticleDto
			{
				Author = new AuthorDto
				{
					FirstName = user.FirstName,
					LastName = user.LastName
				},
				Id = article.Id,
				Content = article.Content,
				Description = article.Description,
				PublishedAt = article.PublishedAt,
				Title = article.Title
			};

			return articleDto;
		}

		/// <inheritdoc/>
		public async Task<bool> IsOwnerAsync(int articleId, string userId)
		{
			var article = await _articleRepository.GetArticleByIdAsync(articleId);
			if (article == null)
			{
				return false;
			}

			var isOwner = article.Author.Id == userId;
			return isOwner;
		}

		/// <inheritdoc/>
		public async Task<ArticleDto> UpdateAsync(int id, ArticleDto articleDto)
		{
			var article = await _articleRepository.GetArticleByIdAsync(id);
			article.Title = articleDto.Title;
			article.Description = articleDto.Description;
			article.Content = articleDto.Content;

			var updatedArticle = await _articleRepository.UpdateAsync(article);

			return new ArticleDto
			{
				Author = new AuthorDto
				{
					FirstName = updatedArticle.Author.FirstName,
					LastName = updatedArticle.Author.LastName
				},
				Id = updatedArticle.Id,
				Content = updatedArticle.Content,
				PublishedAt = updatedArticle.PublishedAt,
				Description = updatedArticle.Description,
				Title = updatedArticle.Title
			};
		}

		/// <inheritdoc/>
		public async Task<bool> DeleteAsync(int id)
		{
			return await _articleRepository.DeleteAsync(id);
		}
	}
}
