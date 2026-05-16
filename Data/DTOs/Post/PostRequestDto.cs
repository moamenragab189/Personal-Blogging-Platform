using System.ComponentModel.DataAnnotations;

namespace Personal_Blogging_Platform.Data.DTOs.Post
{
    public class PostRequestDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}
