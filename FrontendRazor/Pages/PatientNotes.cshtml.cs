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
        public List<NoteResponse> Notes { get; set; }

        [BindProperty]
        public Patient Patient { get; set; }

        [BindProperty]
        public NoteRequest newNote { get; set; } = new NoteRequest();

        public async Task OnGetAsync(int id)
        {
            var client = _httpClientFactory.CreateClient("GatewayClient");
            // Récupérer le jeton d'authentification à partir des cookies
            var authToken = HttpContext.Request.Cookies["authToken"];
            if (!string.IsNullOrEmpty(authToken))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
            }
            Notes = await client.GetFromJsonAsync<List<NoteResponse>>($"/note/patient/{id}");// je recupere l'id de la note egalement , pour update ou delete
            Patient = await client.GetFromJsonAsync<Patient>($"patient/{id}");
        }

        public async Task<IActionResult> OnPostAsync(NoteRequest newNote)
        {
            var client = _httpClientFactory.CreateClient("GatewayClient");
            var authToken = HttpContext.Request.Cookies["authToken"];

            if (!string.IsNullOrEmpty(authToken))
            { 
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
            }
            var response = await client.PostAsJsonAsync("/api/note", newNote);
            if (response.IsSuccessStatusCode)
            {
                var createdNote = await response.Content.ReadFromJsonAsync<NoteRequest>();
                if (createdNote == null)
                {
                    // recharger les données du patient

                }
                if (createdNote != null)
                {
                    Notes = await client.GetFromJsonAsync<List<NoteResponse>>($"/note/patient/{createdNote.PatientId}");
                }

                // Réinitialiser l'objet côté serveur
                this.newNote = new NoteRequest();
                ModelState.Clear(); // Nettoyer l'état du modèle

                // Recharger les données du patient
                await OnGetAsync(newNote.PatientId);

                return Page();
            }
            else
            {
                ModelState.AddModelError(string.Empty, "An error occurred while adding the note.");
                await OnGetAsync(newNote.PatientId);
                return Page();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(string noteId, int patientId)
        {
            var client = _httpClientFactory.CreateClient("GatewayClient");
            var authToken = HttpContext.Request.Cookies["authToken"];
            if (!string.IsNullOrEmpty(authToken))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
            }

            var response = await client.DeleteAsync($"/api/note/{noteId}");
            if (response.IsSuccessStatusCode)
            {
                Notes = await client.GetFromJsonAsync<List<NoteResponse>>($"/note/patient/{patientId}");

                // Recharger les données du patient
                await OnGetAsync(patientId);

                return Page();
            }
            else
            {
                ModelState.AddModelError(string.Empty, "An error occurred while deleting the note.");
                return Page();
            }
        }


        public IActionResult OnPostRedirectToDonneesPatient(int patientId)
        {
            return RedirectToPage("/DonneesPatient", new { id = patientId });
        }
    }
}
