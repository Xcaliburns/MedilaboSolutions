using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Ajouter la configuration Ocelot
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// Ajouter les services Ocelot
builder.Services.AddOcelot(builder.Configuration);

// Ajouter les services de contrôleur si nécessaire
builder.Services.AddControllers();

// Configurer l'authentification basée sur les cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)//.addJwtBearer(options => {})
    .AddCookie(options =>
    {
        options.LoginPath = "/auth/login"; // Correspond à /api/auth/login dans AuthController
        options.LogoutPath = "/auth/logout"; // Correspond à /api/auth/logout dans AuthController
    });

var app = builder.Build();

// Configurer le pipeline de requêtes HTTP
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Utiliser le middleware Ocelot
app.UseOcelot().Wait();

app.Run();
