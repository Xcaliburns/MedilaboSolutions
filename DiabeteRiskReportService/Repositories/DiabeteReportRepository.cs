
using DiabeteRiskReportService.Interfaces;
using DiabeteRiskReportService.Models;
using System.Text.Json;
using System.Threading.Tasks;



namespace DiabeteRiskReportService.Repository
{
    public class DiabeteReportRepository : IDiabeteReportRepository
    {
        private readonly HttpClient _httpClient;

        public DiabeteReportRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("PatientService");
        }

        public async Task<string> GetData(int patientId)
        {
            // Implementation of the method ReportService
            return await Task.FromResult("Report generated successfully");
        }

        public async Task<Patient> GetPatientData(int patientId, string authToken)
        {
            if (!string.IsNullOrEmpty(authToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
            }

            var response = await _httpClient.GetAsync($"/patient/{patientId}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                // Log or inspect the JSON content here
                Console.WriteLine(json); // Or use any logging mechanism
                return JsonSerializer.Deserialize<Patient>(json);
            }
            return null;
        }

        public async Task<List<PatientNote>> GetPatientNotes(int patientId, string authToken)
        {
            if (!string.IsNullOrEmpty(authToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
            }

            var response = await _httpClient.GetAsync($"/note/patient/{patientId}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                // Log or inspect the JSON content here
                Console.WriteLine(json); // Or use any logging mechanism
                return JsonSerializer.Deserialize<List<PatientNote>>(json);
            }
            return null;
        }
    }
}
