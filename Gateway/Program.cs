using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Gateway.Events;

var builder = WebApplication.CreateBuilder(args);

// Add Ocelot configuration
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// Add Ocelot services
builder.Services.AddOcelot(builder.Configuration);


builder.Services.AddControllers();

// Configure JWT authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer("Bearer", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]))
    };
    options.EventsType = typeof(CustomJwtBearerEvents);
});

builder.Services.AddScoped<CustomJwtBearerEvents>();

builder.Services.AddHttpClient("AuthenticatedClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7214");
}).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    UseCookies = true
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        builder =>
        {
            builder.WithOrigins(
                
                "http://localhost:5000",   // Gateway HTTP
                "http://localhost:5010",  // FrontendRazor
                "http://localhost:8080",  // PatientService
                "http://localhost:8090",  // PatientNotes
                "http://localhost:5020"   // DiabeteRiskReportService
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials(); // Autoriser les cookies si besoin
        });
});


// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

app.UseCors("AllowSpecificOrigins");

app.UseAuthentication();
app.UseAuthorization();

// Middleware to log the presence of the authToken cookie
app.Use(async (context, next) =>
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    if (context.Request.Cookies.ContainsKey("authToken"))
    {
        var cookie = context.Request.Cookies["authToken"];
        logger.LogInformation("authToken cookie is present in the request.");
        logger.LogInformation($"authToken cookie value: {cookie}");
    }
    else
    {
        logger.LogInformation("authToken cookie is NOT present in the request.");
    }
    await next.Invoke();
    logger.LogInformation("Finished handling request.");
});

await app.UseOcelot();

app.Run();
