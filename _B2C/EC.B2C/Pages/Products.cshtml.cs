using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EC.B2C.Pages;

public class ProductsModel : PageModel
{
    public void OnGet() => ViewData["Title"] = "產品";
}
