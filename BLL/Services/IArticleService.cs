using BLL.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Services
{
	public interface IArticleService
	{
		/// <summary>
		/// Inserts article and the given user as author in the system.
		/// </summary>
		/// <param name="articleDto">Article dto with the data needed for insert.</param>
		/// <param name="userId">User id that will be treated as the user of the article.</param>
		/// <returns>Inserted article dto.</returns>
		Task<ArticleDto> InsertAsync(ArticleDto articleDto, string userId);
		/// <summary>
		/// Gets article by id from the system.
		/// </summary>
		/// <param name="id">Id of the article that will be retrieved.</param>
		/// <returns>Populated article dto.</returns>
		Task<ArticleDto> GetArticleByIdAsync(int id);
		/// <summary>
		/// Gets paginated newest articles from the system.
		/// </summary>
		/// <param name="pageSize">Page size of the pagination.</param>
		/// <param name="page">Requested page of the pagination.</param>
		/// <returns>Newest articles and pagination data (total count, total page count etc).</returns>
		Task<ArticlesDto> GetNewestArticlesAsync(int pageSize = 10, int page = 1);
		/// <summary>
		/// Checks if provided author is owner of the given article.
		/// </summary>
		/// <param name="articleId">Article id that will be checked.</param>
		/// <param name="authorId">Author id that will be checked.</param>
		/// <returns>True if given author is owner of the article.</returns>
		Task<bool> IsOwnerAsync(int articleId, string authorId);
		/// <summary>
		/// Deletes article from the system.
		/// </summary>
		/// <param name="id">Id of the article that will deleted.</param>
		/// <returns>True if deleted successfuly.</returns>
		Task<bool> DeleteAsync(int id);
		/// <summary>
		/// Updates article in the system. 
		/// </summary>
		/// <param name="id">Id of the article which will be updated.</param>
		/// <param name="articleDto">Article data that will be updated.</param>
		/// <returns>Updated populated article data.</returns>
		Task<ArticleDto> UpdateAsync(int id, ArticleDto articleDto);
	}
}
