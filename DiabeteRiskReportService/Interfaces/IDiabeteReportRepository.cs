using DiabeteRiskReportService.Models;

namespace DiabeteRiskReportService.Interfaces
{
    public interface IDiabeteReportRepository
    {
        Task<string> GetData(int patientId);
        Task<PatientDto> GetPatientData(int patientId, string authToken);
        Task<List<PatientNote>> GetPatientNotes(int patientId, string authToken);

    }
}
