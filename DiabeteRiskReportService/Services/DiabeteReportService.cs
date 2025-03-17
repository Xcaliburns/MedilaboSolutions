using DiabeteRiskReportService.Interfaces;
using DiabeteRiskReportService.Models;
using System.Text.Json;


namespace DiabeteRiskReportService.Services
{
    public class DiabeteReportService : IDiabeteReportService
    {
        private readonly IDiabeteReportRepository _diabeteReportRepository;

        public DiabeteReportService(IDiabeteReportRepository diabeteReportRepository)
        {
            _diabeteReportRepository = diabeteReportRepository;
        }

        public async Task<string> Report(int patientId, string authToken)
        {
            // Récupérer les données du patient
            var patient = await _diabeteReportRepository.GetPatientData(patientId, authToken);
            if (patient != null)
            {
                if (patient.DateDeNaissance == default || string.IsNullOrEmpty(patient.Nom) || string.IsNullOrEmpty(patient.Prenom) || string.IsNullOrEmpty(patient.Genre))
                {
                    return "Patient data is incomplete";
                }

                var dateDeNaissance = patient.DateDeNaissance;
                var age = DateTime.Now.Year - dateDeNaissance.Year;
                // Utiliser la date de naissance pour générer le rapport
                var report = $"Le patient {patient.Nom} {patient.Prenom} a {age} ans et est de sexe {patient.Genre}";
                return report;
            }
            return "Patient not found";
        }
    }
}
