using Frontend.Models;

namespace Frontend.Pages
{
    public partial class Home
    {

       
private List<Patient>? patients;
        private bool isAuthenticated = false;
        private string adress = "https://localhost:7214/patient";

        protected override async Task OnInitializedAsync()
        {
            var result = await DataService.GetAuthenticatedDataAsync<List<Patient>>(adress);
            isAuthenticated = result.isAuthenticated;
            if (isAuthenticated)
            {
                patients = result.data;
            }
        }

        private void NavigateToLogin()
        {
            RedirectToLogin.RedirectToLoginPage();
        }
    }
}
