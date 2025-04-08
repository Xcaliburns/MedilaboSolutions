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
            var patient = await _diabeteReportRepository.GetPatientData(patientId, authToken);
            if (patient != null)
            {
                Console.WriteLine($"Patient ID: {patientId}, Data retrieved: {JsonSerializer.Serialize(patient)}");

                if (patient.DateDeNaissance == default || string.IsNullOrEmpty(patient.Nom) || string.IsNullOrEmpty(patient.Prenom) || string.IsNullOrEmpty(patient.Genre))
                {
                    Console.WriteLine($"Patient ID: {patientId}, Data is incomplete.");
                    return "Patient data is incomplete";
                }

                var dateDeNaissance = patient.DateDeNaissance;
                var age = DateTime.Now.Year - dateDeNaissance.Year;
                var report = $"Le patient {patient.Nom} {patient.Prenom} a {age} ans et est de sexe {patient.Genre}";

                Console.WriteLine($"Patient ID: {patientId}, Report generated: {report}");
                return report;
            }

            Console.WriteLine($"Patient ID: {patientId} not found.");
            return "Patient not found";
        }


        public async Task<int> getTriggersNumberAsync(int patientId, string authToken)
        {
            var triggerList = await _diabeteReportRepository.GetPatientNotes(patientId, authToken);
            var triggersToCheck = new List<string> { "Hémoglobine A1C", "Microalbumine", "Taille", "Poids", "Fumeur", "Fumeuse", "Anormal", "Cholestérol", "Vertiges", "Rechute", "Réaction", "Anticorps" };

            if (triggerList == null || triggerList.Count == 0)
            {
                Console.WriteLine($"Patient ID: {patientId}, Trigger list is empty.");
                return 0;
            }

            int triggerNumber = triggerList
                .SelectMany(note => triggersToCheck, (note, trigger) => new { note, trigger })
                .Count(x => x.note.Note.Contains(x.trigger, StringComparison.OrdinalIgnoreCase));

            Console.WriteLine($"Patient ID: {patientId}, TriggerNumber calculated: {triggerNumber}");
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

                Console.WriteLine($"Patient ID: {patientId}, Age: {age}, Genre: {genre}, TriggerNumber: {triggerNumber}");

                if (age > 0)
                {
                    if (triggerNumber >= 2 && triggerNumber <= 5 && age > 30)
                    {
                        riskLevel = "Borderline";
                    }
                    else if (genre == "H" && age <= 30 && triggerNumber >= 3)
                    {
                        riskLevel = "In Danger";
                    }
                    else if (genre == "F" && age <= 30 && triggerNumber >= 4 && triggerNumber < 7)
                    {
                        riskLevel = "In Danger";
                    }
                    else if (age > 30 && triggerNumber >= 6 && triggerNumber <= 7)
                    {
                        riskLevel = "In Danger";
                    }
                    else if (genre == "H" && age <= 30 && triggerNumber >= 5)
                    {
                        riskLevel = "Early Onset";
                    }
                    else if (genre == "F" && age <= 30 && triggerNumber >= 7)
                    {
                        riskLevel = "Early Onset";
                    }
                    else if (age > 30 && triggerNumber >= 8)
                    {
                        riskLevel = "Early Onset";
                    }
                }
            }

            Console.WriteLine($"Patient ID: {patientId}, Final Risk Level: {riskLevel}");
            return riskLevel;
        }

    }
}
