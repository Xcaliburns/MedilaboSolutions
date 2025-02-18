using Microsoft.JSInterop;
using System.Net.Http.Json;

namespace Frontend.Services
{
    public class DataService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IJSRuntime _jsRuntime;

        public DataService(IHttpClientFactory clientFactory, IJSRuntime jsRuntime)
        {
            _clientFactory = clientFactory;
            _jsRuntime = jsRuntime;
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
            return !string.IsNullOrEmpty(token);
        }

        public async Task<HttpClient> GetAuthenticatedClientAsync()
        {
            var client = _clientFactory.CreateClient("AuthenticatedClient");
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            return client;
        }

        public async Task<List<T>> GetDataAsync<T>(string endpoint)
        {
            var client = await GetAuthenticatedClientAsync();
            return await client.GetFromJsonAsync<List<T>>(endpoint);
        }

        public async Task<T> GetSingleDataAsync<T>(string endpoint)
        {
            var client = await GetAuthenticatedClientAsync();
            return await client.GetFromJsonAsync<T>(endpoint);
        }

        public async Task SaveDataAsync<T>(string endpoint, T data)
        {
            var client = await GetAuthenticatedClientAsync();
            await client.PutAsJsonAsync(endpoint, data);
        }

        public async Task<(bool isAuthenticated, T? data)> GetAuthenticatedDataAsync<T>(string endpoint)
        {
            var isAuthenticated = await IsAuthenticatedAsync();
            if (isAuthenticated)
            {
                var data = await GetSingleDataAsync<T>(endpoint);
                return (true, data);
            }
            return (false, default);
        }
    }
}
