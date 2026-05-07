using CakeShop.Core.DTOs;
using CakeShop.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CakeShop.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactController : ControllerBase
{
    private readonly IContactService _contactService;

    public ContactController(IContactService contactService)
    {
        _contactService = contactService;
    }

    /// <summary>提交聯絡表單</summary>
    [HttpPost]
    public async Task<ActionResult<ContactFormResponse>> Submit([FromBody] ContactFormDto form)
        => Ok(await _contactService.SubmitFormAsync(form));
}
