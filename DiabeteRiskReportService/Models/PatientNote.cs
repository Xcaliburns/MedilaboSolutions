using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DiabeteRiskReportService.Models
{
    public class PatientNote
    {
        [JsonPropertyName("patientId")]
        public int PatientId { get; set; }

        [JsonPropertyName("patientNote")]
        public string Note { get; set; }
    }
}
