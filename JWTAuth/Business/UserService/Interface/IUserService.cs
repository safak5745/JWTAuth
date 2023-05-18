using JWTAuth.Models;

namespace JWTAuth.Business.UserService.Interface;

public interface IUserService
{
    Task<List<User>> GetAllUser();
    Task<User> GetByUserId(Guid id);
}