using System;

namespace DAL.Models.EntityModels
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublishedAt { get; set; }
        public string Content { get; set; }
        public User Author { get; set; }
    }
}
