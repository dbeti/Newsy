using System;

namespace BLL.Models.Dtos
{
	public class ArticleDto
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime PublishedAt { get; set; }
		public string Content { get; set; }
		public AuthorDto Author { get; set; }
	}
}
