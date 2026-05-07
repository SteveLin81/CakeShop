using CakeShop.Core.DTOs;
using CakeShop.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CakeShop.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnnouncementController : ControllerBase
{
    private readonly IAnnouncementService _announcementService;

    public AnnouncementController(IAnnouncementService announcementService)
    {
        _announcementService = announcementService;
    }

    /// <summary>取得目前啟用的置頂公告</summary>
    [HttpGet]
    public async Task<ActionResult<AnnouncementDto>> Get()
    {
        var result = await _announcementService.GetActiveAnnouncementAsync();
        return result is not null ? Ok(result) : NoContent();
    }
}
