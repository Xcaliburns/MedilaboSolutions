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
        //private string adress = "https://localhost:7214/patient/{0}";

        protected override async Task OnInitializedAsync()
        {
            var result = await DataService.GetAuthenticatedDataAsync<Patient>($"/patient/{Id}");
            isAuthenticated = result.isAuthenticated;
            if (isAuthenticated)
            {
                patient = result.data;
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
