using BLL.Services;
using DAL.Models.EntityModels;
using DAL.Repositories;
using Moq;
using System;
using Xunit;

namespace NewsyTests
{
	public class ArticleServiceTests
	{
		[Fact]
		public async void GivenArticle_WhenICheckOwnershipWithCorrectAuthor_ThenArticleIsOwnedByThatAuthor()
		{
			// Given
			var articleRepositoryMoq = new Mock<IArticleRepository>();

			articleRepositoryMoq.Setup(x => x.GetArticleByIdAsync(It.IsAny<int>()))
				.ReturnsAsync(new Article
				{
					Author = new User
					{
						Id = "1"
					}
				});

			// When 
			var articleService = new ArticleService(articleRepositoryMoq.Object, null);
			var isOwner = await articleService.IsOwnerAsync(1, "1");

			// Then
			Assert.True(isOwner);
		}

		[Fact]
		public async void GivenArticle_WhenICheckOwnershipWithWrongAuthor_ThenArticleIsNotOwnedByThatAuthor()
		{
			// Given
			var articleRepositoryMoq = new Mock<IArticleRepository>();

			articleRepositoryMoq.Setup(x => x.GetArticleByIdAsync(It.IsAny<int>()))
				.ReturnsAsync(new Article
				{
					Author = new User
					{
						Id = "1"
					}
				});

			// When 
			var articleService = new ArticleService(articleRepositoryMoq.Object, null);
			var isOwner = await articleService.IsOwnerAsync(1, "2");

			// Then
			Assert.False(isOwner);
		}
	}
}
