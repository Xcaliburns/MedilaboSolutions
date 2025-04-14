﻿using System;
using System.ComponentModel.DataAnnotations;

namespace FrontendRazor.Models

{
    public class Patient
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le champ Nom est requis.")]
        [StringLength(50)]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Le champ Prénom est requis.")]
        [StringLength(50)]
        public string Prenom { get; set; }

        [Required(ErrorMessage = "Le champ Date de Naissance est requis.")]
        public DateTime DateDeNaissance { get; set; }

        [Required(ErrorMessage = "Le champ Genre est requis.")]
        [StringLength(50)]
        [RegularExpression("^(H|F)$", ErrorMessage = "Le champ Genre doit être 'H' pour Homme ou 'F' pour Femme.")]
        public string Genre { get; set; }

       
        public string? Adresse { get; set; }

        [StringLength(50)]
        public string? Telephone { get; set; }
    }
}
