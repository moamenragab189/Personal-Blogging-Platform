using System.ComponentModel.DataAnnotations;

namespace Personal_Blogging_Platform.Data.DTOs.Comment
{
    public class CommentResponseDto
    {
        public int Id { get; set; }

        public int AuthorId { get; set; }
        [Required]
        public int PostId { get; set; }
        [Required]
        public string Content { get; set; }
        
       
    }
}
