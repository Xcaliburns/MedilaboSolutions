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

            if (!ModelState.IsValid)
            {
                // Reload the patient data and notes if the model state is invalid
                Patient = await client.GetFromJsonAsync<Patient>($"patient/{newNote.PatientId}");
                Notes = await client.GetFromJsonAsync<List<NoteResponse>>($"/note/patient/{newNote.PatientId}");
                return Page();
            }

            var response = await client.PostAsJsonAsync("/api/note", newNote);
            if (response.IsSuccessStatusCode)
            {
                var createdNote = await response.Content.ReadFromJsonAsync<NoteRequest>();
                if (createdNote != null)
                {
                    Notes = await client.GetFromJsonAsync<List<NoteResponse>>($"/note/patient/{createdNote.PatientId}");
                }

                // Reset the newNote object
                this.newNote = new NoteRequest();

                // Reload the page to apply the changes
                return RedirectToPage();
            }
            else
            {
                ModelState.AddModelError(string.Empty, "An error occurred while adding the note.");
                // Reload the patient data and notes if there is an error
                Patient = await client.GetFromJsonAsync<Patient>($"patient/{newNote.PatientId}");
                Notes = await client.GetFromJsonAsync<List<NoteResponse>>($"/note/patient/{newNote.PatientId}");
                return Page();
            }
        }



        public IActionResult OnPostRedirectToDonneesPatient(int patientId)
        {
            return RedirectToPage("/DonneesPatient", new { id = patientId });
        }
    }
}
