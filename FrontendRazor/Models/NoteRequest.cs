using System.ComponentModel.DataAnnotations;

namespace FrontendRazor.Models
{
    public class NoteRequest
    {
        [Required]
        public int PatientId { get; set; }
        [Required (ErrorMessage ="il n 'y a pas de message")]

        public string PatientNote { get; set; }
    }
}
