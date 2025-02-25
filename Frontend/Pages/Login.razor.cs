using Frontend.Models;
using Frontend.Services;

namespace Frontend.Pages
{
    public partial class Login
    {
        private LoginModel loginModel = new LoginModel();

        private async Task HandleLogin()
        {
            await AuthService.Login(loginModel);
        }
    }
}
