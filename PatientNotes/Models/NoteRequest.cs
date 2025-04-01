using System.ComponentModel.DataAnnotations;

namespace PatientNotes.Models
{
    public class NoteRequest
    {
        [Required]
        public int PatientId { get; set; }
        [Required]
        public string PatientNote { get; set; }
    }
}
