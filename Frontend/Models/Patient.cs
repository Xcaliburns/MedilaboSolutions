namespace Frontend.Models;
using System;
using System.ComponentModel.DataAnnotations;


public class Patient
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Nom { get; set; }

    [Required]
    [StringLength(50)]
    public string Prenom { get; set; }

    [Required]
    public DateTime DateDeNaissance { get; set; }

    [Required]
    [StringLength(50)]
    public string Genre { get; set; }

    [StringLength(50)]
    public string Adresse { get; set; }

    [StringLength(50)]
    public string Telephone { get; set; }
}

