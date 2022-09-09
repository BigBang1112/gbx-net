using GbxExplorer.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net;
using System.Security.Claims;

namespace GbxExplorer.Client;

public class DiscordAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _client;
    private readonly ISettingsService _settings;

    public DiscordAuthenticationStateProvider(HttpClient client, ISettingsService settings)
    {
        _client = client;
        _settings = settings;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (!_settings.InsiderMode)
        {
            return BecomeInsider();
        }

        using var response = await _client.GetAsync("insider");

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            return new AuthenticationState(new());
        }

        if (response.StatusCode != HttpStatusCode.OK)
        {
            return new AuthenticationState(new(NewIdentity()));
        }

        return BecomeInsider();
    }

    private static AuthenticationState BecomeInsider()
    {
        var identity = NewIdentity();
        identity.AddClaim(new(ClaimTypes.Role, "Insider"));
        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    private static ClaimsIdentity NewIdentity()
    {
        return new ClaimsIdentity(nameof(DiscordAuthenticationStateProvider));
    }
}