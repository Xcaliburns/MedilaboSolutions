using Microsoft.AspNetCore.Mvc.RazorPages;
using FrontendRazor.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

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

        public List<Patient> Patients { get; set; }

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient("GatewayClient");

            // Récupérer le jeton d'authentification à partir des cookies
            var authToken = HttpContext.Request.Cookies["authToken"];
            if (!string.IsNullOrEmpty(authToken))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
            }

            Patients = await client.GetFromJsonAsync<List<Patient>>("patient");
        }
    }

   
}
