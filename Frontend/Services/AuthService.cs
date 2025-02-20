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
        private readonly RedirectToLogin RedirectToLogin;

        public AuthService(HttpClient httpClient, IJSRuntime jsRuntime, NavigationManager navigationManager, RedirectToLogin redirectToLogin)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
            _navigationManager = navigationManager;
            RedirectToLogin = redirectToLogin;
        }

        public async Task<bool> Login(LoginModel loginModel)
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7214/auth/login", loginModel);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                if (result != null && result.TryGetValue("token", out var token))
                {
                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", token);
                    _navigationManager.NavigateTo("/");
                    return true;
                }
            }
            Console.WriteLine("Login failed");
            return false;
        }

        public async Task Logout()
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
            RedirectToLogin.RedirectToLoginPage();   
        }
    }
}
