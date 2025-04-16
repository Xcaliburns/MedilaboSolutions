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
        // Retrieve the token from cookies
        var token = _httpContextAccessor.HttpContext?.Request.Cookies["authToken"];

        // Add the Authorization header if the token is present
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        // Send the request to the next handler
        var response = await base.SendAsync(request, cancellationToken);

        return response;
    }
}

