
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedilaboSolutionsBack1.Models
{
    public class Patient
    {
        public int Id { get; set; }
        [Required]
        public string Nom { get; set; }
        [Required]
        public string Prenom { get; set; }
        [Required]
        public DateTime DateDeNaissance { get; set; }
        [Required]
        // uniquement H ou F
        [RegularExpression("^(H|F)$", ErrorMessage = "Le champ Genre doit être 'H' pour Homme ou 'F' pour Femme.")]
        public string Genre { get; set; }
       
        public int? AdresseId { get; set; }
        public string? Telephone { get; set; }

        [ForeignKey("AdresseId")]
        public virtual Adresse? Adresse { get; set; } // Navigation property
    }
}
