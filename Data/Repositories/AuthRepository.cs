using Microsoft.EntityFrameworkCore;
using Personal_Blogging_Platform.Data.Entities;

namespace Personal_Blogging_Platform.Data.Repositories
{
    public class AuthRepository
    {
        private readonly AppDbContext _context;
        public AuthRepository(AppDbContext context)
        {
            _context = context;
        }
        internal async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        internal async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        internal async Task SaveOTP(OTP userOTP)
        {
           _context.OTPs.Add(userOTP);
           await _context.SaveChangesAsync();
        }
    }
}
