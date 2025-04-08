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
        public Patient Patient { get; set; }

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



            Notes = await client.GetFromJsonAsync<List<NoteResponse>>($"note/patient/{id}");
            Console.WriteLine($"Requ�te envoy�e : note/patient/{id}");
            Patient = await client.GetFromJsonAsync<Patient>($"patient/{id}");
            Console.WriteLine($" Requ�te envoy�e : patient/{id}");
            var riskLevelResponse = await GetPatientRiskLevelAsync(id);
            ViewData["RiskLevel"] = riskLevelResponse.RiskLevel;
        }
           

        public async Task<IActionResult> OnPostAddNoteAsync(NoteRequest newNote) //Attention � ne pas mettre un nom comme OnOstAsync, cela peut creer des ambiguit�s
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
                    // recharger les donn�es du patient

                }
                if (createdNote != null)
                {
                    Notes = await client.GetFromJsonAsync<List<NoteResponse>>($"/note/patient/{createdNote.PatientId}");
                }

                // R�initialiser l'objet c�t� serveur
                this.newNote = new NoteRequest();
                ModelState.Clear(); // Nettoyer l'�tat du mod�le

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
               // Notes = await client.GetFromJsonAsync<List<NoteResponse>>($"/note/patient/{patientId}");

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

            // Log the raw response content
            Console.WriteLine($"Response content: {responseContent}");

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
                        // Log the null response
                        Console.WriteLine("Deserialization returned null.");
                        return new RiskLevelResponse { RiskLevel = "Error parsing risk level response" };
                    }
                }
                catch (JsonException ex)
                {
                    // Log the exception
                    Console.WriteLine($"JsonException: {ex.Message}");
                    return new RiskLevelResponse { RiskLevel = "Error parsing risk level response" };
                }
            }
            else
            {
                // Log the error response
                Console.WriteLine($"Error response: {responseContent}");
                return new RiskLevelResponse { RiskLevel = "Unable to retrieve risk level" };
            }
        }
    }
}
