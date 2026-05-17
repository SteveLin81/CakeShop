using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EC.B2E.Pages.Admin;

public class DashboardModel : PageModel
{
    public void OnGet() => ViewData["Title"] = "儀表板";
}
