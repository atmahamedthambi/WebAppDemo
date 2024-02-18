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
    public partial class UserRegistration
    {
        [Inject]
        public IHttpRequestService HttpRequestService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [SupplyParameterFromForm]
        public UserRegistrationModel UserRegistrationModel { get; set; } = new();

        [CascadingParameter]
        public HttpContext HttpContext { get; set; } = default!;


        //load all the roles during page load

        protected override async Task OnInitializedAsync()
        {
            var request = new ApiRequestModel()
            {
                ApiType = ApiType.Get,
                ApiUrl = ApiHelper.AuthWebApiUrl + "api/auth/roles"
            };
            var response = await HttpRequestService.SendAsync(request, false);
            if(response != null && response.IsSuccess)
            {
                UserRegistrationModel.Roles = JsonConvert.DeserializeObject<List<RolesModel>>(Convert.ToString(response.Result));
            }
        }

        private async Task OnRegistrationFormSubmit()
        {
            // here we need to validate user input and call our authweb api endpoint to save user registration details
            var request = new ApiRequestModel()
            {
                ApiType = ApiType.Post,
                ApiUrl = ApiHelper.AuthWebApiUrl + "api/auth",
                Data = UserRegistrationModel
            };
            if (UserRegistrationModel != null)
            {
                var response = await HttpRequestService.SendAsync(request, false);
                if (response != null && response.IsSuccess)
                {
                    var userModel = JsonConvert.DeserializeObject<UserModel>(Convert.ToString(response.Result));
                    var handler = new JwtSecurityTokenHandler();
                    var jwt = handler.ReadJwtToken(userModel.Token);
                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    if (jwt != null && jwt.Claims != null && jwt.Claims.Any())
                    {
                        foreach (var item in jwt.Claims)
                        {
                            identity.AddClaim(new Claim(item.Type, item.Value));
                        }
                    }
                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    NavigationManager.NavigateTo("/");
                }
            }
        }
    }
}
