using System.Net.Http.Headers;

public class AuthTokenHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthTokenHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Récupérer le token depuis les cookies
        var token = _httpContextAccessor.HttpContext?.Request.Cookies["authToken"];

        // Ajouter l'en-tête Authorization si le token est présent
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        // Journaliser les détails de la requête
        Console.WriteLine($"Request: {request.Method} {request.RequestUri}");
        if (request.Content != null)
        {
            var content = await request.Content.ReadAsStringAsync();
            Console.WriteLine($"Request Content: {content}");
        }

        // Envoyer la requête au prochain gestionnaire
        var response = await base.SendAsync(request, cancellationToken);

        // Journaliser les détails de la réponse
        Console.WriteLine($"Response: {response.StatusCode}");
        if (response.Content != null)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response Content: {responseContent}");
        }

        return response;
    }
}

