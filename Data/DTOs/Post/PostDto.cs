using System.ComponentModel.DataAnnotations;

namespace Personal_Blogging_Platform.Data.DTOs.Post
{
    public class PostDto
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}
