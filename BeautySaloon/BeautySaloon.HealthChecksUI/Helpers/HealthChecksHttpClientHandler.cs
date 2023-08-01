using IdentityModel.Client;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Serilog;

namespace BeautySaloon.HealthChecksUI.Helpers;

internal class HealthChecksHttpClientHandler : DelegatingHandler
{
    private readonly HealthChecksAuthSettings _options;
    private readonly IMemoryCache _memoryCache;

    private DiscoveryDocumentResponse _discoveryResponse;

    public HealthChecksHttpClientHandler(IOptions<HealthChecksAuthSettings> options,
                                         IMemoryCache memoryCache)
    {
        _options = options.Value;
        _memoryCache = memoryCache;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Log.Information(_options.Authority);
        if (_discoveryResponse == null)
        {
            _discoveryResponse = await new HttpClient().GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _options.Authority,
                Policy = new DiscoveryPolicy
                {
                    RequireHttps = false
                }
            }, cancellationToken);

            if (_discoveryResponse.IsError)
            {
                throw new Exception("Could not retrieve discovery document.", _discoveryResponse.Exception);
            }
        }

        if (!_memoryCache.TryGetValue(nameof(HealthChecksHttpClientHandler), out TokenResponse tokenResponse))
        {
            var response = await new HttpClient()
                                         .RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                                         {
                                             Address = _discoveryResponse.TokenEndpoint,
                                             ClientId = _options.ClientId,
                                             ClientSecret = _options.ClientSecret
                                         }, cancellationToken: cancellationToken);

            if (response.IsError)
            {
                throw new Exception("Could not retrieve access token.", response.Exception);
            }

            var tokenExpiration = response.ExpiresIn == 0
                ? TimeSpan.MaxValue
                : TimeSpan.FromSeconds(response.ExpiresIn);

            var cacheExpiration = tokenExpiration > TimeSpan.FromHours(1)
                ? TimeSpan.FromHours(1)
                : tokenExpiration.Subtract(TimeSpan.FromMinutes(1));

            _memoryCache.Set(nameof(HealthChecksHttpClientHandler), response, cacheExpiration);
            tokenResponse = response;
        }

        var authorizationHeader = $"Bearer {tokenResponse.AccessToken}";
        request.Headers.Add(HeaderNames.Authorization, authorizationHeader);
        Log.Debug("Adding authorization header '{authorizationHeader}'", authorizationHeader);

        var responseMessage = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        return responseMessage;
    }
}