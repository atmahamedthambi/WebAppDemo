using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace OpenIdWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        [HttpPost("~/token")]
        public async ValueTask<IActionResult> Exchange()
        {
            var request = HttpContext.GetOpenIddictServerRequest();
            if(request.IsClientCredentialsGrantType())
            {
                var clientId = request.ClientId;
                var identity = new ClaimsIdentity(authenticationType: TokenValidationParameters.DefaultAuthenticationType);
                identity.SetClaim(Claims.Subject, clientId);
                identity.SetScopes(request.GetScopes());
                var principal = new ClaimsPrincipal(identity);
                return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }
            await Task.CompletedTask;
            return null;
        }
    }
}
