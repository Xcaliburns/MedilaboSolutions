using FrontendRazor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontendRazor.Pages
{

    [Authorize(Roles = "Organisateur, Praticien")]
    public class DossierPatientModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DossierPatientModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public Patient Patient { get; set; }

        public async Task OnGetAsync(int id)
        {
            var client = _httpClientFactory.CreateClient("GatewayClient");

            // Récupérer le jeton d'authentification à partir des cookies
            var authToken = HttpContext.Request.Cookies["authToken"];
            if (!string.IsNullOrEmpty(authToken))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
            }

            Patient = await client.GetFromJsonAsync<Patient>($"patient/{id}");
         
        }

        public async Task<IActionResult> OnPostAsync()
        {
           
            if (!ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("ModelState is not valid");
                return Page();
            }

            var client = _httpClientFactory.CreateClient("GatewayClient");

            // Récupérer le jeton d'authentification à partir des cookies
            var authToken = HttpContext.Request.Cookies["authToken"];
            if (!string.IsNullOrEmpty(authToken))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
            }
            else
            {
                return RedirectToLogin();
            }

            var response = await client.PutAsJsonAsync($"patient/edit/{Patient.Id}", Patient);

            if (response.IsSuccessStatusCode)
            {
                System.Diagnostics.Debug.WriteLine("Update successful");
                return RedirectToPage("/Index");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToLogin();
            }

            // Gérer les erreurs de mise à jour
            System.Diagnostics.Debug.WriteLine("Update failed");
            ModelState.AddModelError(string.Empty, "Erreur lors de la mise à jour des données du patient.");
            return Page();
        }

        private IActionResult RedirectToLogin()
        {
            return RedirectToPage("/Login");
        }
    }
}
