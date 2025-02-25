using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontendRazor.Pages
{
    public class _LayoutModel : PageModel
    {
        public IActionResult OnPostLogout()
        {
            // Effacer les cookies d'authentification
            Response.Cookies.Delete("authToken");

            // Rediriger vers la page de connexion
            return RedirectToPage("/Login");
        }
    }
}
