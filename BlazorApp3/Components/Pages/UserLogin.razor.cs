using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BlazorApp3;
using BlazorApp3.Models;
using BlazorApp3.Service.IService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;

namespace BlazorApp3.Components.Pages
{
    public partial class UserLogin
    {
        [Inject]
        public IHttpRequestService httpRequestService { get; set; }

        [CascadingParameter]
        public HttpContext httpContext { get; set; } = default!;

        [Inject]
        public AuthenticationStateProvider ? authenticationStateProvider { get; set; }

        [Inject]
        public NavigationManager? NavigationManager { get; set; }

        [Inject]
        public IHttpContextAccessor? httpContextAccessor { get; set; }

        private UserLoginModel UserLoginModel { get; set; } = new();
        

        private async Task OnUserLoginFormSubmit()
        {
            try
            {
                var request = new ApiRequestModel()
                {
                    ApiType = ApiType.Post,
                    ApiUrl = ApiHelper.AuthWebApiUrl + "api/auth/login",
                    Data = UserLoginModel
                };
                if (UserLoginModel != null)
                {
                    var response = await httpRequestService.SendAsync(request);
                    if (response != null && response.IsSuccess)
                    {
                        var userModel = JsonConvert.DeserializeObject<UserModel>(Convert.ToString(response.Result));
                        var handler = new JwtSecurityTokenHandler();
                        var jwt = handler.ReadJwtToken(userModel.Token);
                        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                        identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, jwt.Claims.First(x => x.Type == JwtRegisteredClaimNames.Email).Value));
                        identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name, jwt.Claims.First(x => x.Type == JwtRegisteredClaimNames.Name).Value));
                        identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.First(x => x.Type == JwtRegisteredClaimNames.Email).Value));
                        var principal = new ClaimsPrincipal(identity);

                        //var handler = new JwtSecurityTokenHandler();
                        //var jwt = handler.ReadJwtToken(userModel.Token);
                        //var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                        //identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, jwt.Claims.First(x => x.Type == JwtRegisteredClaimNames.Email).Value));
                        //identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name, jwt.Claims.First(x => x.Type == JwtRegisteredClaimNames.Name).Value));
                        //identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.First(x => x.Type == JwtRegisteredClaimNames.Email).Value));

                        //var principal = new ClaimsPrincipal(identity);
                        //await httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                        //var state = await authenticationStateProvider.GetAuthenticationStateAsync();

                        NavigationManager.NavigateTo("/");
                    }
                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }
        }
    }
}
