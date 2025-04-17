using Microsoft.AspNetCore.Mvc;
using FrontendRazor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

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
        public PatientDto Patient { get; set; }

        [BindProperty]
        public NoteRequest newNote { get; set; } = new NoteRequest();

       



        public async Task OnGetAsync(int id)
        {
            var client = _httpClientFactory.CreateClient("GatewayClient");
            var authToken = HttpContext.Request.Cookies["authToken"];
            if (!string.IsNullOrEmpty(authToken))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
            }

            try
            {
                // R�cup�ration des notes du patient
                var notesResponse = await client.GetFromJsonAsync<List<NoteResponse>>($"note/patient/{id}");
                Notes = notesResponse ?? new List<NoteResponse>();
                Console.WriteLine($" Requ�te envoy�e : note/patient/{id}");

                // R�cup�ration des informations du patient
                var patientResponse = await client.GetFromJsonAsync<PatientDto>($"patient/{id}");
                Patient = patientResponse ?? new PatientDto();
                Console.WriteLine($" Requ�te envoy�e : patient/{id}");

                // R�cup�ration du niveau de risque du patient
                var riskLevelResponse = await GetPatientRiskLevelAsync(id);
                ViewData["RiskLevel"] = riskLevelResponse?.RiskLevel ?? "Inconnu";
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($" Erreur lors de la r�cup�ration des donn�es du patient : {ex.Message}");
                ModelState.AddModelError(string.Empty, "Impossible de r�cup�rer les donn�es du patient.");
            }
        }



        public async Task<IActionResult> OnPostAddNoteAsync(NoteRequest newNote)
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
                    Notes = await client.GetFromJsonAsync<List<NoteResponse>>($"/note/patient/{newNote.PatientId}") ?? new List<NoteResponse>();

                }
                if (createdNote != null)
                {
                    Notes = await client.GetFromJsonAsync<List<NoteResponse>>($"/note/patient/{createdNote.PatientId}") ?? new List<NoteResponse>();
                }

                // R�initialiser l'objet c�t� serveur
                this.newNote = new NoteRequest();
                ModelState.Clear();

                // Recharger les donn�es du patient
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
                // Recharger les donn�es du patient
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

        public async Task<RiskLevelResponse> GetPatientRiskLevelAsync(int patientId)
        {
            var client = _httpClientFactory.CreateClient("GatewayClient");
            var authToken = HttpContext.Request.Cookies["authToken"];
            if (!string.IsNullOrEmpty(authToken))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
            }

            var response = await client.GetAsync($"/report/{patientId}");
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var riskLevelResponse = JsonSerializer.Deserialize<RiskLevelResponse>(responseContent);
                    if (riskLevelResponse != null)
                    {
                        return riskLevelResponse;
                    }
                    else
                    {
                        return new RiskLevelResponse { RiskLevel = "Error parsing risk level response" };
                    }
                }
                catch (JsonException ex)
                {
                    return new RiskLevelResponse { RiskLevel = "Error parsing risk level response" };
                }
            }
            else
            {
                return new RiskLevelResponse { RiskLevel = "Unable to retrieve risk level" };
            }
        }
    }
}
