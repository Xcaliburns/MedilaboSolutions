using DiabeteRiskReportService.Interfaces;
using DiabeteRiskReportService.Models;
using System.Drawing;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;
using static System.Reflection.Metadata.BlobBuilder;


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


                if (age > 0)
                {
                    //Borderline:Le dossier du patient contient entre deux et cinq déclencheurs et le patient est âgé de plus de 30 ans
                    if (triggerNumber >= 2 && triggerNumber <= 5 && age > 30)
                    {
                        riskLevel = "Borderline";
                    }
                    //In Danger:Si le patient est un homme de moins de 30 ans, alors trois termes déclencheurs doivent être présents
                    else if (genre == "H" && age <= 30 && triggerNumber >= 3)
                    {
                        riskLevel = "In Danger";
                    }
                    //In Danger:  Si le patient est une femme et a moins de 30 ans, il faudra quatre termes déclencheurs
                    else if (genre == "F" && age <= 30 && triggerNumber >= 4 && triggerNumber < 7)
                    {
                        riskLevel = "In Danger";
                    }
                    //In Danger:  Si le patient a plus de 30 ans, alors il en faudra six ou sept;
                    else if (age > 30 && triggerNumber >= 6 && triggerNumber <= 7)
                    {
                        riskLevel = "In Danger";
                    }
                    //Early Onset:  Si le patient est un homme de moins de 30 ans, alors au moins cinq termes déclencheurs sont nécessaires. 
                    else if (genre == "H" && age <= 30 && triggerNumber >= 5)
                    {
                        riskLevel = "Early Onset";
                    }
                    //Early Onset:   Si le patient est une femme et a moins de 30 ans, il faudra au moins sept termes déclencheurs
                    else if (genre == "F" && age <= 30 && triggerNumber >= 7)
                    {
                        riskLevel = "Early Onset";
                    }
                    //Early Onset: Si le patient a plus de 30 ans, alors il en faudra huit ou plus.
                    else if (age > 30 && triggerNumber >= 8)
                    {
                        riskLevel = "Early Onset";
                    }

                }
                return riskLevel;
            }
            return riskLevel;
        }
    }
}
