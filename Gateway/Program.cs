using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Ajouter la configuration Ocelot
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// Ajouter les services Ocelot
builder.Services.AddOcelot(builder.Configuration);

// Ajouter les services de contr�leur si n�cessaire
builder.Services.AddControllers();

// Configurer l'authentification bas�e sur les cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)//.addJwtBearer(options => {})
    .AddCookie(options =>
    {
        options.LoginPath = "/auth/login"; // Correspond � /api/auth/login dans AuthController
        options.LogoutPath = "/auth/logout"; // Correspond � /api/auth/logout dans AuthController
    });

var app = builder.Build();

// Configurer le pipeline de requ�tes HTTP
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Utiliser le middleware Ocelot
app.UseOcelot().Wait();

app.Run();
