using DAL.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
	public interface IArticleRepository
	{
		/// <summary>
		/// Gets newest articles.
		/// </summary>
		/// <param name="take">How many articles will be returned.</param>
		/// <param name="skip">How many articles will be skipped.</param>
		/// <returns>Populated articles ordered by <see cref="Article.PublishedAt"/>.</returns>
		Task<IEnumerable<Article>> GetNewestArticlesAsync(int take = 10, int skip = 0);
		/// <summary>
		/// Gets article by id.
		/// </summary>
		/// <param name="id">Id of the article.</param>
		/// <returns>Populated article.</returns>
		Task<Article> GetArticleByIdAsync(int id);
		/// <summary>
		/// Inserts article.
		/// </summary>
		/// <param name="article">Article which will be inserted in the database.</param>
		/// <returns>Inserted populated article.</returns>
		Task<Article> InsertAsync(Article article);
		/// <summary>
		/// Updates article.
		/// </summary>
		/// <param name="article">Article that will be updated.</param>
		/// <returns>Updated populated article.</returns>
		Task<Article> UpdateAsync(Article article);
		/// <summary>
		/// Deletes article.
		/// </summary>
		/// <param name="id">Id of the article that will be deleted.</param>
		/// <returns>True if deletion was successful, false otherwise.</returns>
		Task<bool> DeleteAsync(int id);
		/// <summary>
		/// Gets total number of the articles.
		/// </summary>
		/// <returns>Number of articles in the database.</returns>
		Task<int> GetArticlesCountAsync();
	}
}
