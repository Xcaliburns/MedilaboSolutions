using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PatientNotes.Data;
using PatientNotes.Interfaces;
using PatientNotes.Models;
using PatientNotes.Repository;
using PatientNotes.Services;
using System.Text;
using MongoDB.Driver;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

//  **Configuration de la base SQL Server pour Identity**
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

//  **Configuration et injection de `PatientNotesDatabaseSettings` via `IOptions<>`**
builder.Services.Configure<PatientNotesDatabaseSettings>(
    builder.Configuration.GetSection("MedilaboNotesData"));

//  **Ajout de l'injection des paramètres MongoDB**
builder.Services.AddSingleton(sp =>
{
    var settings = sp.GetRequiredService<IOptions<PatientNotesDatabaseSettings>>().Value;

    if (string.IsNullOrEmpty(settings.ConnectionString))
    {
        throw new InvalidOperationException(" MongoDB ConnectionString est NULL. Vérifie l’injection des paramètres.");
    }

    Console.WriteLine($" ConnectionString MongoDB utilisée : {settings.ConnectionString}");

    var client = new MongoClient(settings.ConnectionString);
    var database = client.GetDatabase(settings.DatabaseName);

    Console.WriteLine($" Base de données MongoDB : {database.DatabaseNamespace}");

    return database.GetCollection<Note>(settings.CollectionName);
});

//  **Injection des services**
builder.Services.AddSingleton<INotesService, NotesService>();
builder.Services.AddSingleton<INotesRepository, NotesRepository>();

//  **Ajout des controllers et API Explorer**
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

//  **Configuration Swagger avec JWT**
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Medilabo API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

//  **Configuration de l'authentification JWT**
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

//  **Configuration des politiques d'autorisation**
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("PraticienPolicy", policy =>
        policy.RequireRole("Praticien"));
});

//  **Construction de l'application**
var app = builder.Build();

//  **Ajout de Swagger si en mode développement**
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Medilabo API v1");
    });
}

//  **Middleware d'authentification et d'autorisation**
app.UseAuthentication();
app.UseAuthorization();

//  **Mapping des contrôleurs**
app.MapControllers();

//  **Lancement de l'application**
app.Run();
