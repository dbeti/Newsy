using Microsoft.EntityFrameworkCore;
using DAL.Models.EntityModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
	/// <summary>
	/// Implementation of the basic article operations on the database using entity framework core. 
	/// </summary>
	public class ArticleRepository : IArticleRepository
	{
		private readonly NewsyDbContext _newslyDbContext;
		public ArticleRepository(NewsyDbContext newslyDbContext)
		{
			_newslyDbContext = newslyDbContext;
		}

		/// <inheritdoc/>
		public async Task<bool> DeleteAsync(int id)
		{
			var article = await _newslyDbContext.Articles.FindAsync(id);
			_newslyDbContext.Articles.Remove(article);
			var saveChangeResult = await _newslyDbContext.SaveChangesAsync();

			return saveChangeResult > 0;
		}

		/// <inheritdoc/>
		public async Task<Article> GetArticleByIdAsync(int id)
		{
			var article = await _newslyDbContext.Articles
				.Include(x => x.Author)
				.Where(x => x.Id == id)
				.FirstOrDefaultAsync();

			return article;
		}

		/// <inheritdoc/>
		public async Task<IEnumerable<Article>> GetNewestArticlesAsync(int take, int skip)
		{
			var articles = await _newslyDbContext.Articles
				.AsNoTracking()
				.Include(x => x.Author)
				.OrderByDescending(x => x.PublishedAt)
				.Skip(skip)
				.Take(take)
				.ToListAsync();

			return articles;
		}

		/// <inheritdoc/>
		public async Task<int> GetArticlesCountAsync()
		{
			var articlesCount = await _newslyDbContext.Articles.CountAsync();
			return articlesCount;
		}

		/// <inheritdoc/>
		public async Task<Article> InsertAsync(Article article)
		{
			await _newslyDbContext.Articles.AddAsync(article);
			await _newslyDbContext.SaveChangesAsync();

			return article;
		}

		/// <inheritdoc/>
		public async Task<Article> UpdateAsync(Article article)
		{
			_newslyDbContext.Articles.Update(article);
			await _newslyDbContext.SaveChangesAsync();

			return article;	
		}
	}
}
