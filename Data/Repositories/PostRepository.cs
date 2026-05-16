using Microsoft.EntityFrameworkCore;
using Personal_Blogging_Platform.Data.Entities;

namespace Personal_Blogging_Platform.Data.Repositories
{
    public class PostRepository
    {
        AppDbContext _context;
        public PostRepository(AppDbContext context)
        {
            _context = context;
        }
        internal async Task AddPostAsync(Post post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
        }

        internal async Task<List<Post>> GetPostsAsync()
        {
                return await _context.Posts.ToListAsync();
        }
    }
}
