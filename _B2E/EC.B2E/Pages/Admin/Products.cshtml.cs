using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EC.B2E.Pages.Admin;

public class ProductsAdminModel : PageModel
{
    public void OnGet() => ViewData["Title"] = "商品管理";
}
