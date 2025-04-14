namespace MedilaboSolutionsBack1.Models
{
    public class Adresse
    {
        public int Id { get; set; }
        public string? Libele { get; set; }

        public ICollection<Patient>? Patients { get; set; } 
    }
}
