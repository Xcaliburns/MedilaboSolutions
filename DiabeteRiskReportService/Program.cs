using DiabeteRiskReportService.Interfaces;
using DiabeteRiskReportService.Repository;
using DiabeteRiskReportService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PatientNotes.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<IDiabeteReportService, DiabeteReportService>();
builder.Services.AddScoped<IDiabeteReportRepository, DiabeteReportRepository>();


// Configure DbContext with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity services
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Configure JWT authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
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
});

// Configuration des politiques d'autorisation
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("PraticienPolicy", policy =>
        policy.RequireRole("Praticien"));
});

// Add HttpClient services
builder.Services.AddHttpClient();

builder.Services.AddHttpClient("PatientService", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["GatewayUrl"]);
});

builder.Services.AddHttpClient("NoteService", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["GatewayUrl"]);
});

// Configure CORS avec le appsettings.Json
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", corsBuilder =>
    {
        corsBuilder.WithOrigins(builder.Configuration["GatewayUrl"])
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();
    });
});

// Register Swagger services before building the app
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseCors("CorsPolicy"); // Apply CORS policy

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
