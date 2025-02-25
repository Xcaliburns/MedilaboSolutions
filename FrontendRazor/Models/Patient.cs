using System;
using System.ComponentModel.DataAnnotations;

namespace FrontendRazor.Models

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

        public string Adresse { get; set; }
        public string Telephone { get; set; }
    }
}
