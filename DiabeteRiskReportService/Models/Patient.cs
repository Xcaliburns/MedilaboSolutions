using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DiabeteRiskReportService.Models
{
    public class Patient
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le champ Nom est requis.")]
        [StringLength(50)]
        [JsonPropertyName("nom")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Le champ Prénom est requis.")]
        [StringLength(50)]
        [JsonPropertyName("prenom")]
        public string Prenom { get; set; }

        [Required(ErrorMessage = "Le champ Date de Naissance est requis.")]
        [JsonPropertyName("dateDeNaissance")]
        public DateTime DateDeNaissance { get; set; }

        [Required(ErrorMessage = "Le champ Genre est requis.")]
        [StringLength(50)]
        [RegularExpression("^(H|F)$", ErrorMessage = "Le champ Genre doit être 'H' pour Homme ou 'F' pour Femme.")]
        [JsonPropertyName("genre")]
        public string Genre { get; set; }

        [StringLength(50)]
        [JsonPropertyName("adresse")]
        public string? Adresse { get; set; }

        [StringLength(50)]
        [JsonPropertyName("telephone")]
        public string? Telephone { get; set; }
    }
}
