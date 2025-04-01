namespace DiabeteRiskReportService.Interfaces
{
    public interface IDiabeteReportService
    {
        Task<string> Report(int patientId, string authToken);
    }
}
