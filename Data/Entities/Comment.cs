using System.ComponentModel.DataAnnotations.Schema;

namespace Personal_Blogging_Platform.Data.Entities
{
    public class Comment: BaseEntity
    {

        public string Content { get; set; }
        [ForeignKey("User")]
        public int AuthorId { get; set; }
        [ForeignKey("Post")]
        public int PostId { get; set; }
        public User User { get; set; }
        public Post Post { get; set; }
    }
}
