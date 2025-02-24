namespace Frontend.Models;
using System;
using System.ComponentModel.DataAnnotations;


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
    public DateTime DateDeNaissance { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "Le champ Genre est requis.")]
    [StringLength(50)]
    public string Genre { get; set; }

    [StringLength(50)]
    public string Adresse { get; set; }

    [StringLength(50)]
    public string Telephone { get; set; }
}

