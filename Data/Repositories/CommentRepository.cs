using Microsoft.EntityFrameworkCore;
using Personal_Blogging_Platform.Data.Entities;

namespace Personal_Blogging_Platform.Data.Repositories
{
    public class CommentRepository
    {
        private readonly AppDbContext _context;
        public CommentRepository(AppDbContext context)
        {
            _context = context;
        }
            

        internal async Task AddComment(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
        }

        internal async Task<Comment> GetCommentByIdAsync(int id)
        {
            return await _context.Comments.FindAsync(id);
        }

        internal async Task DeleteComment(Comment comment)
        {
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }

        internal async Task<List<Comment>> GetCommentsByPostIdAsync(int postId)
        {
            return await _context.Comments.Where(c => c.PostId == postId).ToListAsync();
        }
    }
}
