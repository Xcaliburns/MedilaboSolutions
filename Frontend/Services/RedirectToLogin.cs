using Microsoft.AspNetCore.Components;

namespace Frontend.Services
{
    public class RedirectToLogin
    {
        private readonly NavigationManager _navigationManager;
        public RedirectToLogin(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }
        public void RedirectToLoginPage()
        {
            _navigationManager.NavigateTo("login");
        }
    }
}
