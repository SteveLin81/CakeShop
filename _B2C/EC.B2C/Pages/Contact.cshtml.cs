using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EC.B2C.Pages;

public class ContactModel : PageModel
{
    public void OnGet() => ViewData["Title"] = "聯絡我們";
}
