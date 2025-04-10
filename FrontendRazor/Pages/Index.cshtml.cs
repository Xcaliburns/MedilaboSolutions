using FrontendRazor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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

        public List<PatientDto> Patients { get; set; } = new List<PatientDto>();
        public bool IsAuthenticated { get; private set; }
        public bool IsAuthorized { get; private set; }

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient("GatewayClient");

            var authToken = HttpContext.Request.Cookies["authToken"];
            if (!string.IsNullOrEmpty(authToken))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
            }

            try
            {
                Patients = await client.GetFromJsonAsync<List<PatientDto>>("patient"); // Adapter au format attendu
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, "Une erreur s'est produite lors de la récupération des patients.");
            }

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
