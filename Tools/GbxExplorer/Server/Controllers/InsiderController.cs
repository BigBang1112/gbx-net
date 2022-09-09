using AspNet.Security.OAuth.Discord;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GbxExplorer.Server.Controllers;

[Route("insider")]
[ApiController]
public class InsiderController : ControllerBase
{
    private readonly IConfiguration _config;

    public InsiderController(IConfiguration config)
    {
        _config = config;
    }

    public IActionResult Get()
    {
        if (_config.GetValue<bool>("InsiderMode") == false)
        {
            return NotFound();
        }

        if (User.Identity is not ClaimsIdentity identity || !identity.IsAuthenticated)
        {
            return Unauthorized();
        }

        var insiders = _config.GetSection("Insiders").Get<string[]>();

        var identifier = identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

        if (identifier is null)
        {
            return Forbid();
        }

        if (insiders.Contains(identifier.Value))
        {
            return Ok();
        }

        return Forbid();
    }
}
