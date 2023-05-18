using JWTAuth.Models;

namespace JWTAuth.Business.AuthService.Interface
{
    public interface IAuthService
    {
        public Task<string> Login(string email, string password);
        public Task<User> Register(User user);
    }
}
