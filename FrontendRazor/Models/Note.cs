namespace FrontendRazor.Models
{
    public class Note
    {
        public string _id { get; set; }
        public int PatientId { get; set; }
        public string PatientNote { get; set; }
    }
}
