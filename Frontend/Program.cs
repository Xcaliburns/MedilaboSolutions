using Frontend;
using Frontend.Helpers;
using Frontend.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Enregistrer AuthenticatedHttpClientHandler
builder.Services.AddTransient<AuthenticatedHttpClientHandler>();

// Configure HttpClient to use the Ocelot Gateway URL for the named client
builder.Services.AddHttpClient("AuthenticatedClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7214");
});

// Configure default HttpClient
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("AuthenticatedClient"));

builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("Local", options.ProviderOptions);
    options.UserOptions.RoleClaim = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
});

builder.Services.AddScoped<DataService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<RedirectToLogin>();

await builder.Build().RunAsync();
