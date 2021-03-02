using System.Collections.Generic;

namespace BLL.Models.Dtos
{
	public class ArticlesDto
	{
		public IEnumerable<ArticleDto> Articles { get; set; }
		public int CurrentPage { get; set; }
		public int PageSize { get; set; }
		public int Count { get; set; }
		public int TotalPages { get; set; }
	}
}
