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
            // Sign out the user
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            //delete .AspNetCore.Cookies cookie
            HttpContext.Response.Cookies.Delete(".AspNetCore.Cookies");
            // Delete the authToken cookie
            HttpContext.Response.Cookies.Delete("authToken");

            return RedirectToPage("/Index");
        }
    }
}
