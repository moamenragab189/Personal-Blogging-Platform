using System.ComponentModel.DataAnnotations;

namespace Personal_Blogging_Platform.Data.Entities
{
    public class User: BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string HashedPassword { get; set; }
        public bool IsEmailVerified { get; set; }= false;
        public List<Post> Posts { get; set; }= new List<Post>();
        public List<Comment> Comments { get; set; }= new List<Comment>();

    }
}
