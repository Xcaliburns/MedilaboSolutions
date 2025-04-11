using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontendRazor.Pages
{
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnPostAsync()
        {
           
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            
            HttpContext.Response.Cookies.Delete(".AspNetCore.Cookies");
           
            HttpContext.Response.Cookies.Delete("authToken");

            return RedirectToPage("/Index");
        }
    }
}
