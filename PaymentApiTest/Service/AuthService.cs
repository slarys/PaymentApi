using Microsoft.IdentityModel.Tokens;
using PaymentApiTest.Data;
using PaymentApiTest.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace PaymentApi.Service
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public bool ValidateCredentials(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return false;
            }

            var user = _context.Users.SingleOrDefault(u => u.Username == username && u.PasswordHash == password);
            return user != null;
        }

        public bool Logout(int userId, int transactionId)
        {
            var user = _context.Users.Include(u => u.Transactions).SingleOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return false;
            }

            var transaction = user.Transactions.SingleOrDefault(t => t.Id == transactionId);

            if (transaction != null)
            {
                user.Transactions.Remove(transaction);
                _context.SaveChanges();
                return true;
            }

            return false;
        }



    }
}
