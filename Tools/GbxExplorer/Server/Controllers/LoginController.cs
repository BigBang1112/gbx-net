using AspNet.Security.OAuth.Discord;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace GbxExplorer.Server.Controllers;

[Route("login")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly IConfiguration _config;

    public LoginController(IConfiguration config)
    {
        _config = config;
    }

    public IActionResult Get()
    {
        if (_config.GetValue<bool>("InsiderMode"))
        {
            return Challenge(new AuthenticationProperties { RedirectUri = "/" }, DiscordAuthenticationDefaults.AuthenticationScheme);
        }

        return NotFound();
    }
}
