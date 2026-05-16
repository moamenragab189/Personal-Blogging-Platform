using Personal_Blogging_Platform.Data.DTOs.Comment;
using System.ComponentModel.DataAnnotations;

namespace Personal_Blogging_Platform.Data.DTOs.Post
{
    public class PostResponseDto
    {
        public int Id { get; set; }
        [Required]
        public int AuthorId { get; set; }
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public List<CommentResponseDto> Comments { get; set; }= new List<CommentResponseDto>();
    }
}
