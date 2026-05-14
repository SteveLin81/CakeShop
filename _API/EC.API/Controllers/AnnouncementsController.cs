using CakeShop.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EC.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AnnouncementsController : ControllerBase
{
    private readonly IAnnouncementService _announcementService;

    public AnnouncementsController(IAnnouncementService announcementService)
        => _announcementService = announcementService;

    /// <summary>取得目前生效中的公告</summary>
    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        var announcement = await _announcementService.GetActiveAnnouncementAsync();
        return announcement is null
            ? NotFound(new { message = "目前無生效中的公告" })
            : Ok(announcement);
    }
}
