using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Personal_Blogging_Platform.Data.Entities
{
    public class Post: BaseEntity
    {
        [Required]
        public string Title { get; set; }
        [ForeignKey("User")]
        public int AuthorId { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        [Required]
        public string Content { get; set; }
        public User User { get; set; }
        public Category Category { get; set; }
        public List<Comment> Comments { get; set; }= new List<Comment>();
    }
}
