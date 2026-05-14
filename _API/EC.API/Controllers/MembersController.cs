using CakeShop.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EC.API.Controllers;

public record MemberResponse(int Id, string Username, string Email, DateTime CreatedAt);

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class MembersController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public MembersController(IUserRepository userRepository)
        => _userRepository = userRepository;

    /// <summary>依使用者名稱查詢會員</summary>
    [HttpGet("{username}")]
    public async Task<IActionResult> GetByUsername(string username)
    {
        var user = await _userRepository.GetByUsernameAsync(username);
        return user is null
            ? NotFound(new { message = $"會員 '{username}' 不存在" })
            : Ok(new MemberResponse(user.Id, user.Username, user.Email, user.CreatedAt));
    }

    /// <summary>依會員 ID 查詢</summary>
    [HttpGet("id/{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user is null
            ? NotFound(new { message = $"會員 ID {id} 不存在" })
            : Ok(new MemberResponse(user.Id, user.Username, user.Email, user.CreatedAt));
    }
}
