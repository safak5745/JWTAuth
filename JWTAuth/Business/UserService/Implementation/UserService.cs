using JWTAuth.Business.UserService.Interface;
using JWTAuth.Models;
using Microsoft.EntityFrameworkCore;

namespace JWTAuth.Business.UserService.Implementation;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _dbContext;

    public UserService(ApplicationDbContext dbContext) => _dbContext = dbContext;

    public async Task<List<User>> GetAllUser() => await _dbContext.Users.Where(x => x.IsActive).ToListAsync();

    public async Task<User> GetByUserId(Guid id) => await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
}