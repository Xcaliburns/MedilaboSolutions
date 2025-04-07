using FrontendRazor.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();


//builder.Services.AddHttpClient("GatewayClient", client =>
//{
//    client.BaseAddress = new Uri("http://gateway:5000/");
//})
//.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
//{
//    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
//});

builder.Services.AddTransient<AuthTokenHandler>();


builder.Services.AddHttpClient("GatewayClient", client =>
{
    client.BaseAddress = new Uri("http://gateway:5000/");
})
.AddHttpMessageHandler<AuthTokenHandler>()
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
});



// **Mise à jour de la configuration CORS**
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        builder =>
        {
            builder.WithOrigins(
               // "https://localhost:5001",  // Gateway HTTPS
                "http://localhost:5000"  // Gateway HTTP               
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
        });
});

// Add Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.LoginPath = "/login";
        options.LogoutPath = "/logout";
        options.AccessDeniedPath = "/login";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowSpecificOrigins");
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

app.Run();
