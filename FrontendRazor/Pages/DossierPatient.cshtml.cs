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

            // Ajoutez des journaux de débogage pour vérifier l'ID du patient
            System.Diagnostics.Debug.WriteLine($"OnGetAsync - Patient ID: {Patient?.Id}");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var id = Patient.Id;         

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

            var response = await client.PutAsJsonAsync($"patient/edit/{id}", Patient);

            if (response.IsSuccessStatusCode)
            {
                System.Diagnostics.Debug.WriteLine("Update successful");
                return RedirectToPage("/Index");
            }

            // Gérer les erreurs de mise à jour
            System.Diagnostics.Debug.WriteLine("Update failed");
            ModelState.AddModelError(string.Empty, "Erreur lors de la mise à jour des données du patient.");
            return Page();
        }
    }
}
