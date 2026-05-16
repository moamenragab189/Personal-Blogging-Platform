using System.ComponentModel.DataAnnotations;

namespace Personal_Blogging_Platform.Data.DTOs.Comment
{
    public class CommentRequestDto
    {
        [Required]
        public int PostId { get; set; }
        [Required]
        public string Content { get; set; }
    }
}
