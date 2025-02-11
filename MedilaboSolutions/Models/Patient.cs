using System;
using System.ComponentModel.DataAnnotations;

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
        public string Genre { get; set; }
        [Required]
        public string Adresse { get; set; }
        public string Telephone { get; set; }
    }
}
