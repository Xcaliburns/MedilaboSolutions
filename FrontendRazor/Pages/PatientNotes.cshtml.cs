using Microsoft.AspNetCore.Mvc;
using FrontendRazor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontendRazor.Pages
{

    [Authorize(Roles = "Praticien")]
    public class PatientNotesModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PatientNotesModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public List<Note> Notes { get; set; }

        public async Task OnGetAsync(int id)
        {
            var client = _httpClientFactory.CreateClient("GatewayClient");
            // Récupérer le jeton d'authentification à partir des cookies
            var authToken = HttpContext.Request.Cookies["authToken"];
            if (!string.IsNullOrEmpty(authToken))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
            }
            Notes = await client.GetFromJsonAsync<List<Note>>($"/note/patient/{id}");
        }
    }
}
