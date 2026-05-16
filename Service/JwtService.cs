using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Personal_Blogging_Platform.Service
{
    public class JwtService
    {
        private readonly IConfiguration _configuration;
        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public List<Claim> AddUserClaims(int id,string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Username cannot be null or empty.");
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.NameIdentifier, id.ToString())
            };
            return claims;
        }
        public string CreateToken(List<Claim> claims)

        {
            if (claims == null || claims.Count == 0)
            {
                throw new ArgumentException("Claims cannot be null or empty.");
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var creds = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256
                );
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(3),
                signingCredentials: creds
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
