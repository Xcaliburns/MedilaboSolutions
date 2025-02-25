using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Components;

namespace Frontend.Pages
{
    public partial class NewPatient
    {
        private Patient newPatient = new Patient();
        private bool isAuthenticated = false;
        private string? securityMessage;

        
        protected override async Task OnInitializedAsync()
        {
            isAuthenticated = await DataService.IsAuthenticatedAsync();
            if (!isAuthenticated)
            {
                securityMessage = "Veuillez vous connecter pour ajouter un patient.";
            }
        }

        private async Task CreateNewPatient()
        {
            if (!isAuthenticated)
            {
                securityMessage = "Par mesure de sécurité, nous vous invitons à vous connecter à nouveau.";
                return;
            }

            try
            {
                await DataService.CreatePatient(newPatient);
                Navigation.NavigateTo("/");
            }
            catch (Exception ex)
            {
                securityMessage = $"Erreur lors de l'ajout du patient: {ex.Message}";
            }
        }

        private void NavigateToLogin()
        {
            Navigation.NavigateTo("/login");
        }
    }
}
