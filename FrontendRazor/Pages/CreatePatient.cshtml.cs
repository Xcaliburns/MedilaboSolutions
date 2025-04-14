namespace FrontendRazor.Pages;
using FrontendRazor.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

public class CreatePatientModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CreatePatientModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [BindProperty]
    public PatientDto Patient { get; set; } = new PatientDto(); 

    public IActionResult OnGet()
    {
        var authToken = HttpContext.Request.Cookies["authToken"];
        if (string.IsNullOrEmpty(authToken))
        {
            return RedirectToLogin();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var client = _httpClientFactory.CreateClient("GatewayClient");
        var authToken = HttpContext.Request.Cookies["authToken"];
        if (!string.IsNullOrEmpty(authToken))
        {
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
        }
        else
        {
            return RedirectToLogin();
        }

        var response = await client.PostAsJsonAsync("patient/create", Patient); 

        if (response.IsSuccessStatusCode)
        {
            return RedirectToPage("/Index");
        }
        else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return RedirectToLogin();
        }

        ModelState.AddModelError(string.Empty, "Erreur lors de la création du patient.");
        return Page();
    }

    private IActionResult RedirectToLogin()
    {
        return RedirectToPage("/Login");
    }
}
