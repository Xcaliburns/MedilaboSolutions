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


            var reportData = new
            {
                PatientId = patientId,
                RiskLevel = riskLevel
            };

            var jsonReport = JsonSerializer.Serialize(reportData);

            return jsonReport;
        }

        public async Task<PatientDto> getPatientAsync(int patientId, string authToken)
        {
            var patient = await _diabeteReportRepository.GetPatientData(patientId, authToken);
            if (patient != null)
            {
                Console.WriteLine($"Patient ID: {patientId}, Data retrieved: {JsonSerializer.Serialize(patient)}");

                if (patient.DateDeNaissance == default || string.IsNullOrEmpty(patient.Nom) || string.IsNullOrEmpty(patient.Prenom) || string.IsNullOrEmpty(patient.Genre))
                {
                    Console.WriteLine($"Patient ID: {patientId}, Data is incomplete.");
                    return null;
                }

                return patient;
            }

            Console.WriteLine($"Patient ID: {patientId} not found.");
            return null;
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
            const string defaultRiskLevel = "None";


            var triggerTask = getTriggersNumberAsync(patientId, authToken);
            var patientTask = getPatientAsync(patientId, authToken);

            await Task.WhenAll(triggerTask, patientTask);

            int triggerNumber = triggerTask.Result;
            
            var patient = patientTask.Result;

            if (patient == null || patient.DateDeNaissance == default || string.IsNullOrEmpty(patient.Nom) || string.IsNullOrEmpty(patient.Prenom) || string.IsNullOrEmpty(patient.Genre))
            {
                
                return defaultRiskLevel;
            }

            int age = DateTime.Now.Year - patient.DateDeNaissance.Year;
            string genre = patient.Genre;

            Console.WriteLine($"Patient ID: {patientId}, Age: {age}, Genre: {genre}, TriggerNumber: {triggerNumber}");

            if (age <= 0) return defaultRiskLevel;

            return DetermineRiskLevel(age, genre, triggerNumber);
        }

        private string DetermineRiskLevel(int age, string genre, int triggerNumber)
        {
            if (age > 30)
            {
                if (triggerNumber >= 2 && triggerNumber <= 5) return "Borderline";
                if (triggerNumber >= 6 && triggerNumber <= 7) return "In Danger";
                if (triggerNumber >= 8) return "Early Onset";
            }
            else
            {
                if (genre == "H" && triggerNumber >= 3) return triggerNumber >= 5 ? "Early Onset" : "In Danger";
                if (genre == "F" && triggerNumber >= 4) return triggerNumber >= 7 ? "Early Onset" : "In Danger";
            }

            return "None";
        }

    }
}
