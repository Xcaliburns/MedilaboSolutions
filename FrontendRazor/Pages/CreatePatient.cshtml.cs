using FrontendRazor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontendRazor.Pages
{
    [Authorize(Roles = "Organisateur, Praticien")]
    public class CreatePatientModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CreatePatientModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public Patient Patient { get; set; } = new Patient();

        public IActionResult OnGet()
        {
            // R�cup�rer le jeton d'authentification � partir des cookies
            var authToken = HttpContext.Request.Cookies["authToken"];
            if (string.IsNullOrEmpty(authToken))
            {
                return RedirectToLogin();
            }        

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var client = _httpClientFactory.CreateClient("GatewayClient");

            // R�cup�rer le jeton d'authentification � partir des cookies
            var authToken = HttpContext.Request.Cookies["authToken"];
            if (!string.IsNullOrEmpty(authToken))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
            }
            else
            {
                return RedirectToLogin();
            }

            var response = await client.PostAsJsonAsync("patient/create", Patient);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Index");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToLogin();
            }

            // G�rer les autres erreurs de cr�ation
            ModelState.AddModelError(string.Empty, "Erreur lors de la cr�ation du patient.");
            return Page();
        }

        private IActionResult RedirectToLogin()
        {
            return RedirectToPage("/Login");
        }
    }
}
