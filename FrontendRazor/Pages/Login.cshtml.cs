using FrontendRazor.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FrontendRazor.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(IHttpClientFactory httpClientFactory, ILogger<LoginModel> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            Login = new FrontendRazor.Models.LoginModel(); // Initialize to avoid null
            ErrorMessage = string.Empty; // Initialize to avoid null
        }

        [BindProperty]
        public FrontendRazor.Models.LoginModel Login { get; set; }

        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Veuillez remplir tous les champs requis.";
                return Page();
            }

            var client = _httpClientFactory.CreateClient("GatewayClient");
            _logger.LogInformation("Sending login request to auth/login");
            var response = await client.PostAsJsonAsync("auth/login", Login);

            _logger.LogInformation("Response Status Code: {StatusCode}", response.StatusCode);
            var responseBody = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("Response Body: {ResponseBody}", responseBody);

            if (response.IsSuccessStatusCode)
            {
                var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
                var authToken = loginResponse?.Token;

                if (authToken != null)
                {
                    HttpContext.Response.Cookies.Append("authToken", authToken, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.UtcNow.AddMinutes(30)
                    });

                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken(authToken);
                    var roles = jwtToken.Claims.Where(c => c.Type == ClaimsIdentity.DefaultRoleClaimType).Select(c => c.Value).ToList();

                    var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, Login.Username),
                                new Claim("AuthToken", authToken)
                            };

                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        AllowRefresh = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
                        IssuedUtc = DateTimeOffset.UtcNow,
                        RedirectUri = "/"
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    return RedirectToPage("/Index");
                }
            }

            ErrorMessage = "Tentative de connexion échouée. Veuillez vérifier vos identifiants.";
            return Page();
        }
    }
}
