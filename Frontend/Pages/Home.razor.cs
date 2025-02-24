using Frontend.Models;

namespace Frontend.Pages
{
    public partial class Home
    {
        private List<Patient>? patients;
        private bool isAuthenticated = false;
        private string adress = "https://localhost:7214/patient";
        private string? errorMessage;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var result = await DataService.GetAuthenticatedDataAsync<List<Patient>>(adress);
                isAuthenticated = result.isAuthenticated;
                if (isAuthenticated)
                {
                    if (result.data != null)
                    {
                        patients = result.data;
                    }
                    else
                    {
                        errorMessage = result.errorMessage ?? "An unknown error occurred.";
                    }
                }
                else
                {
                    errorMessage = "User is not authenticated.";
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"An error occurred: {ex.Message}";
            }
        }

        private void NavigateToLogin()
        {
            RedirectToLogin.RedirectToLoginPage();
        }
    }
}
