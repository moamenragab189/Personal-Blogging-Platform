using System.ComponentModel.DataAnnotations;

namespace Personal_Blogging_Platform.Data.Entities
{
    public class Category: BaseEntity
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Post> Posts { get; set; }= new List<Post>();

    }
}
