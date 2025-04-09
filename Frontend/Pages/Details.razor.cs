using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Components;

namespace Frontend.Pages
{
    public partial class Details
    {
        [Parameter]
        public int Id { get; set; }

        private Patient? patient;
        private bool isAuthenticated = false;
        private string? securityMessage;
       
protected override async Task OnInitializedAsync()
        {
            var result = await DataService.GetAuthenticatedDataAsync<Patient>($"/patient/{Id}");
            isAuthenticated = result.isAuthenticated;
            if (isAuthenticated)
            {
                if (result.data != null)
                {
                    patient = result.data;
                }
                else
                {
                    securityMessage = result.errorMessage ?? "An unknown error occurred.";
                }
            }
            else
            {
                securityMessage = "User is not authenticated.";
            }
        }

        private async Task SaveChanges()
        {
            var result = await DataService.GetAuthenticatedDataAsync<Patient>($"/patient/{Id}");
            isAuthenticated = result.isAuthenticated;
            if (isAuthenticated)
            {
                await DataService.SaveDataAsync($"/patient/edit/{Id}", patient);
                Navigation.NavigateTo("/");

            }
            else
            {
                securityMessage = "Par mesure de sécurité, nous vous invitons à vous connecter à nouveau.";
            }
          
        }


        private void NavigateToLogin()
        {
            RedirectToLogin.RedirectToLoginPage();
        }
    }
}
