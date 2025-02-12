using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Ajouter la configuration Ocelot
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// Ajouter les services Ocelot
builder.Services.AddOcelot(builder.Configuration);

// Ajouter les services de contr�leur si n�cessaire
builder.Services.AddControllers();

var app = builder.Build();

// Configurer le pipeline de requ�tes HTTP
app.UseHttpsRedirection();

app.UseAuthorization();

// Utiliser le middleware Ocelot
app.UseOcelot().Wait();

app.Run();
