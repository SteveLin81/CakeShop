using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EC.B2E.Pages.Admin;

public class UsersAdminModel : PageModel
{
    public void OnGet() => ViewData["Title"] = "B2C 帳號管理";
}
