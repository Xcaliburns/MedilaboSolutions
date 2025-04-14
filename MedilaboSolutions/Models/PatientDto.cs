namespace MedilaboSolutionsBack1.Models
{
    public class PatientDto
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public DateTime DateDeNaissance { get; set; }
        public string Genre { get; set; }
        public string? Telephone { get; set; }
        public AdresseDto? Adresse { get; set; } 
    }

    public class AdresseDto
    {
        public string? Libele { get; set; }
    }
}
