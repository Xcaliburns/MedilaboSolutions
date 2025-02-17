using System.Net.Http.Json;
using Frontend.Models;

namespace Frontend.Services
{
    public class PatientService
    {
        private readonly HttpClient _httpClient;

        public PatientService(HttpClient httpClient) => _httpClient = httpClient;

        public async Task<Patient> GetPatientById(int id)
        {
            return await _httpClient.GetFromJsonAsync<Patient>($"/patient/{id}");
        }

        public async Task<List<Patient>> GetPatients()
        {
            return await _httpClient.GetFromJsonAsync<List<Patient>>("/patient");
        }
    }
}
