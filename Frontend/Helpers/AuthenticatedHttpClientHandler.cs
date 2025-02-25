using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;
using System.Net.Http.Headers;

namespace Frontend.Helpers
{
    public class AuthenticatedHttpClientHandler : DelegatingHandler
    {
        private readonly IAccessTokenProvider _tokenProvider;

        public AuthenticatedHttpClientHandler(IAccessTokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var result = await _tokenProvider.RequestAccessToken();
            if (result.TryGetToken(out var token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
