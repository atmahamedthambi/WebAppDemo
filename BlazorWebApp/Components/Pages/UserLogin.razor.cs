using BlazorWebApp.Models;
using BlazorWebApp.Service.IService;
using BlazorWebApp.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BlazorWebApp.Components.Pages
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
            if(UserLoginModel != null)
            {
                var request = new ApiRequestModel()
                {
                    ApiType = ApiType.Post,
                    ApiUrl = ApiHelper.AuthWebApiUrl + "api/auth/login",
                    Data = UserLoginModel
                };

                var response = await HttpRequestService.SendAsync(request, false);
                if(response != null && response.IsSuccess) 
                {
                    var userModel = JsonConvert.DeserializeObject<UserModel>(Convert.ToString(response.Result));
                    var handler = new JwtSecurityTokenHandler();
                    var jwt = handler.ReadJwtToken(userModel.Token);
                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    if(jwt != null && jwt.Claims != null && jwt.Claims.Any() )
                    {
                        foreach(var item in jwt.Claims)
                        {
                            identity.AddClaim(new Claim(item.Type, item.Value));
                        }
                    }

                    var principal = new ClaimsPrincipal(identity);
                    HttpContext.Response.Cookies.Append("JwtBearerToken", userModel.Token);
                    HttpContext.Response.Cookies.Append("LoggedInUser", userModel.Email);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    NavigationManager.NavigateTo("/");
                }
            }
        }
    }
}
