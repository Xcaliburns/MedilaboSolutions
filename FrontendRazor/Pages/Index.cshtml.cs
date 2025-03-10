using Microsoft.AspNetCore.Mvc.RazorPages;
using FrontendRazor.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrontendRazor.Pages
{
    [Authorize(Roles = "Organisateur, Praticien")]
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<Patient> Patients { get; set; } = new List<Patient>();
        public bool IsAuthenticated { get; private set; }
        public bool IsAuthorized { get; private set; }

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient("GatewayClient");

            // Récupérer le jeton d'authentification à partir des cookies
            var authToken = HttpContext.Request.Cookies["authToken"];
            if (!string.IsNullOrEmpty(authToken))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
            }

            try
            {
                Patients = await client.GetFromJsonAsync<List<Patient>>("patient");
            }
            catch (HttpRequestException ex)
            {
                // Gérer les erreurs de requête HTTP
                ModelState.AddModelError(string.Empty, "Une erreur s'est produite lors de la récupération des patients.");
            }

            // Définir les propriétés IsAuthenticated et IsAuthorized
            var user = HttpContext.User;
            IsAuthenticated = user.Identity.IsAuthenticated;
            IsAuthorized = user.IsInRole("Organisateur") || user.IsInRole("Praticien");
        }

        public IActionResult OnPostRedirectToDonneesPatient(int patientId)
        {
            return RedirectToPage("/DonneesPatient", new { id = patientId });
        }

        public IActionResult OnPostRedirectToPatientNotes(int patientId)
        {
            return RedirectToPage("/PatientNotes", new { id = patientId });
        }
    }

   
}
