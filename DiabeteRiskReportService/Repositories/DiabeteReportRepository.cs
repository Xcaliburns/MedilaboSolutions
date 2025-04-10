
using DiabeteRiskReportService.Interfaces;
using DiabeteRiskReportService.Models;
using System.Text.Json;



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

        public async Task<PatientDto> GetPatientData(int patientId, string authToken)
        {
            if (!string.IsNullOrEmpty(authToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
            }

            var response = await _httpClient.GetAsync($"patient/{patientId}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine($" JSON reçu du patient: {json}"); //  Vérification des données


                try
                {
                    var patient = JsonSerializer.Deserialize<PatientDto>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true //  Désérialisation insensible à la casse
                    });

                    Console.WriteLine($" Patient correctement désérialisé: {JsonSerializer.Serialize(patient)}");
                    return patient;
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($" Erreur de désérialisation : {ex.Message}");
                    return null;
                }
            }
            return null;
        }


        public async Task<List<PatientNote>> GetPatientNotes(int patientId, string authToken)
        {
            if (!string.IsNullOrEmpty(authToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
            }

            var response = await _httpClient.GetAsync($"note/patient/{patientId}");
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
