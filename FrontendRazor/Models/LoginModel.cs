using System.ComponentModel.DataAnnotations;

namespace FrontendRazor.Models
{
    public class LoginModel
    {
        [Required (ErrorMessage = "le champ est obligatoire")]
        [Display(Name = "Nom d'utilisateur")]
        public string Username { get; set; }

        [Required(ErrorMessage = "le champ est obligatoire")]
        [Display(Name = "Mot de Passe")]
        public string Password { get; set; }
    }
}
