using DiabeteRiskReportService.Models;

namespace DiabeteRiskReportService.Interfaces
{
    public interface IDiabeteReportRepository
    {
        Task<string> GetData(int patientId);
        Task<Patient> GetPatientData(int patientId, string authToken); // Nouvelle méthode
    }
}
