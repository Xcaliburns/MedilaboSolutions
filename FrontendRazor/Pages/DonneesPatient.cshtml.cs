using FrontendRazor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontendRazor.Pages
{

    [Authorize(Roles = "Organisateur, Praticien")]
    public class DonneesPatientModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DonneesPatientModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public PatientDto Patient { get; set; } 


        public async Task OnGetAsync(int id)
        {
            var client = _httpClientFactory.CreateClient("GatewayClient");

            var authToken = HttpContext.Request.Cookies["authToken"];
            if (!string.IsNullOrEmpty(authToken))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
            }

            Patient = await client.GetFromJsonAsync<PatientDto>($"patient/{id}"); 
        }


        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("ModelState is not valid");
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

            // G�rer les erreurs de mise � jour (voir pour ajouter un logger)
            System.Diagnostics.Debug.WriteLine("Update failed");
            ModelState.AddModelError(string.Empty, "Erreur lors de la mise � jour des donn�es du patient.");
          

            return Page();
        }

        private IActionResult RedirectToLogin()
        {
            return RedirectToPage("/Login");
        }
    }
}
