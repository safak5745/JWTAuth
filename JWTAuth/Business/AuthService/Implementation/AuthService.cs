using JWTAuth.Models;
using JWTAuth.Business.AuthService.Interface;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTAuth.Business.AuthService.Implementation;
    using BCrypt.Net;
using Microsoft.EntityFrameworkCore;

public class AuthService : IAuthService
    {
        public readonly ApplicationDbContext dbContext;
        public readonly IConfiguration configuration;
        public AuthService(ApplicationDbContext DbContext, IConfiguration Configuration)
        {
            dbContext = DbContext;
            configuration = Configuration;
        }

        public async Task<string> Login(string email, string password)
        {
            User? user = await dbContext.Users.FirstOrDefaultAsync(x=>x.UserName == email);
            if (user == null || BCrypt.Verify(password, user.Password) == false) return null;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration["JWT:SecretKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.GivenName, user.Name),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(180),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            //user.Token = tokenHandler.WriteToken(token);
            //user.IsActive = true;
            return tokenHandler.WriteToken(token);
        }

        public async Task<User> Register(User user)
        {
            if (string.IsNullOrEmpty(user.Role)) return null;
            user.Password = BCrypt.HashPassword(user.Password);
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
            return user;
        }
    }

