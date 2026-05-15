using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EC.B2C.Pages;

public class IndexModel : PageModel
{
    public void OnGet() => ViewData["Title"] = "甜蜜烘焙坊 Sweet Bakes";
}
