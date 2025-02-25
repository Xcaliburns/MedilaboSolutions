using Frontend.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http.Json;

namespace Frontend.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;
        private readonly NavigationManager _navigationManager;
        private readonly RedirectToLogin _redirectToLogin;

        public AuthService(HttpClient httpClient, IJSRuntime jsRuntime, NavigationManager navigationManager, RedirectToLogin redirectToLogin)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _jsRuntime = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
            _navigationManager = navigationManager ?? throw new ArgumentNullException(nameof(navigationManager));
            _redirectToLogin = redirectToLogin ?? throw new ArgumentNullException(nameof(redirectToLogin));
        }

        public async Task<bool> Login(LoginModel loginModel)
        {
            if (loginModel == null) throw new ArgumentNullException(nameof(loginModel));

            var response = await _httpClient.PostAsJsonAsync("https://localhost:7214/auth/login", loginModel);
            if (response.IsSuccessStatusCode)
            {
                // Use JavaScript interop to get the authToken cookie
                var authToken = await _jsRuntime.InvokeAsync<string>("cookieHelper.getCookie", "authToken");
                if (!string.IsNullOrEmpty(authToken))
                {
                    Console.WriteLine("Auth token found in cookies.");
                    _navigationManager.NavigateTo("/");
                    return true;
                }
            }
            Console.WriteLine($"Login failed with status code: {response.StatusCode}");
            Console.WriteLine($"Number of headers: {response.Headers.Count()}");
            foreach (var header in response.Headers)
            {
                Console.WriteLine($"{header.Key}: {string.Join(", ", header.Value)}");
            }
            return false;
        }

        public async Task Logout()
        {
            var response = await _httpClient.PostAsync("https://localhost:7214/auth/logout", null);
            if (response.IsSuccessStatusCode)
            {
                // Use JavaScript interop to erase the authToken cookie
                await _jsRuntime.InvokeVoidAsync("cookieHelper.eraseCookie", "authToken");
                _redirectToLogin.RedirectToLoginPage();
            }
        }
    }
}
