using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EC.B2E.Pages.Admin;

public class CategoriesAdminModel : PageModel
{
    public void OnGet() => ViewData["Title"] = "分類管理";
}
