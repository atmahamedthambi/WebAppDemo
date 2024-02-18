using IdentityBlazorApp.Models;
using IdentityBlazorApp.Service.IService;
using IdentityBlazorApp.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IdentityBlazorApp.Components.Pages
{
    public partial class UserLogin
    {
        [SupplyParameterFromForm]
        private UserLoginModel UserLoginModel { get; set; } = new();

        [CascadingParameter]
        public HttpContext HttpContext { get; set; } = default!;

        [Inject]
        public IHttpRequestService HttpRequestService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        private async Task OnLoginFormSubmit()
        {
            if (UserLoginModel != null)
            {
                var request = new ApiRequestModel()
                {
                    ApiType = ApiType.Post,
                    ApiUrl = ApiHelper.AuthWebApiUrl + "api/auth/login",
                    Data = UserLoginModel
                };

                var response = await HttpRequestService.SendAsync(request);
                if (response != null && response.IsSuccess)
                {
                    var userModel = JsonConvert.DeserializeObject<UserModel>(Convert.ToString(response.Result));
                    var handler = new JwtSecurityTokenHandler();
                    var jwt = handler.ReadJwtToken(userModel.Token);
                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, jwt.Claims.First(x => x.Type == JwtRegisteredClaimNames.Email).Value));
                    identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name, jwt.Claims.First(x => x.Type == JwtRegisteredClaimNames.Name).Value));
                    identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.First(x => x.Type == JwtRegisteredClaimNames.Email).Value));
                    // once we got the role in the token then we need to assign the role in here during signins
                    if (jwt.Claims.Any())
                    {
                        foreach (var item in jwt.Claims)
                        {
                            if (item.Type == "role")
                            {
                                identity.AddClaim(new Claim(ClaimTypes.Role, item.Value));
                            }
                        }
                    }

                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(principal);
                    //await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, principal, new AuthenticationProperties() { AllowRefresh = true });
                    NavigationManager.NavigateTo("/");
                }
            }
        }
    }
}
