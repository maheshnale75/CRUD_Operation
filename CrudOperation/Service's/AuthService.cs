using CrudOperation.DbData;
using CrudOperation.DTO_s;
using CrudOperation.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MontCrestTask.Services
{
    public class AuthService : Interfaces.IAuthService
    {
        private readonly AppData _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppData context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<User> RegisterUser(RegisterUserDTO registeruser, string imagePath)
        {
            if (_context.Users.Any(u => u.EmailId == registeruser.EmailId))
            {
                throw new Exception("A user with this Email Id is already exist");
            }

            var salt = GenerateSalt();
            var hashedPassword = HashPassword(registeruser.Password, salt);

            var user = new User
            {
                UserName = registeruser.UserName,
                HashedPassword = hashedPassword,
                Salt = salt,
                EmailId = registeruser.EmailId,
                MobileNumber = registeruser.MobileNumber,
                Country = registeruser.Country,
                CreatedDateTime = DateTime.UtcNow,
                ProfileImagePath = imagePath,
                RoleId = registeruser.RoleId
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }
        public async Task<string> Login(LoginUserDTO loginDto)
        {
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.EmailId == loginDto.Input || u.UserName == loginDto.Input);
            if (user == null)
                throw new Exception("User not found");

            var hashedPassword = HashPassword(loginDto.Password, user.Salt);
            if (user.HashedPassword != hashedPassword)
                throw new Exception("Invalid credentials");

            var session = new Session
            {
                UserId = user.Id,
                RoleId = user.RoleId, 
                SessionId = Guid.NewGuid(),
                IsActive = true,
                LoginTime = DateTime.UtcNow
            };

            _context.Sessions.Add(session);
            await _context.SaveChangesAsync();

            var token = GenerateJwtToken(session.SessionId.ToString(), user.EmailId, user.Role.Role);

            return token;
        }
        public async Task Logout(Guid sessionId)
        {
            var session = await _context.Sessions.FirstOrDefaultAsync(s => s.SessionId == sessionId);
            if (session == null)
                throw new Exception("Session not found");

            session.IsActive = false;
            session.LogoutTime = DateTime.UtcNow;

            _context.Sessions.Update(session);
            await _context.SaveChangesAsync();
        }
        private string GenerateJwtToken(string sessionId, string name, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.NameIdentifier, sessionId),
                new Claim(ClaimTypes.Role, role), new Claim(ClaimTypes.GivenName,name)
            }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = password + salt;
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
                return Convert.ToBase64String(bytes);
            }
        }
        public string GenerateSalt()
        {
            var rng = new RNGCryptoServiceProvider();
            byte[] saltBytes = new byte[16];
            rng.GetBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }
        
    }
}
