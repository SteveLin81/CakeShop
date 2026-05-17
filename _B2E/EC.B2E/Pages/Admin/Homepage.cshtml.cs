using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EC.B2E.Pages.Admin;

public class HomepageAdminModel : PageModel
{
    public void OnGet() => ViewData["Title"] = "首頁設定";
}
