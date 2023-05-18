using JWTAuth.Business.UserService.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWTAuth.Controllers;

[Route("[controller]/[action]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService) => _userService = userService;

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUserList() => Ok(await _userService.GetAllUser());
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUserById(string id) => Ok(await _userService.GetByUserId(Guid.Parse(id)));
}