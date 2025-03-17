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
            var riskLevel = await GetRiskLevelAsync(patientId, authToken);

            // Create an object to hold the report data
            var reportData = new
            {
                PatientId = patientId,
                RiskLevel = riskLevel
            };

            // Serialize the report data to JSON
            var jsonReport = JsonSerializer.Serialize(reportData);

            return jsonReport;
        }

        public async Task<string> getPatientAsync(int patientId, string authToken)
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

        public async Task<int> getTriggersNumberAsync(int patientId, string authToken)
        {
            var triggerList = await _diabeteReportRepository.GetPatientNotes(patientId, authToken);

            var triggersToCheck = new List<string> { "Hémoglobine A1C", "Microalbumine", "Taille", "Poids", "Fumeur", "Fumeuse", "Anormal", "Cholestérol", "Vertiges", "Rechute", "Réaction", "Anticorps" };

            if (triggerList == null || triggerList.Count == 0)
            {
                return 0;
            }

            int triggerNumber = triggerList
                .SelectMany(note => triggersToCheck, (note, trigger) => new { note, trigger })
                .Count(x => x.note.Note.Contains(x.trigger, StringComparison.OrdinalIgnoreCase));

            return triggerNumber;
        }

        public async Task<string> GetRiskLevelAsync(int patientId, string authToken)
        {
            string riskLevel = "None";
            int triggerNumber = await getTriggersNumberAsync(patientId, authToken);
            var patientReport = await getPatientAsync(patientId, authToken);
            if (patientReport != "Patient not found" && patientReport != "Patient data is incomplete")
            {
                var patient = await _diabeteReportRepository.GetPatientData(patientId, authToken);
                int age = DateTime.Now.Year - patient.DateDeNaissance.Year;
                string genre = patient.Genre;

                if (triggerNumber >= 3)
                {
                    riskLevel = "Early Onset";
                }
                else if (triggerNumber == 2)
                {
                    riskLevel = "In Danger";
                }
                else if (triggerNumber == 1)
                {
                    riskLevel = "Borderline";
                }
                else if (age < 30 && genre == "M")
                {
                    riskLevel = "In Danger";
                }
                else if (age > 30 && genre == "F")
                {
                    riskLevel = "Borderline";
                }

                return riskLevel;
            }
            return riskLevel;
        }
    }
}
