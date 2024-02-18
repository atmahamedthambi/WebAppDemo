using BlazorWebApp.Models;
using BlazorWebApp.Service.IService;
using BlazorWebApp.Utility;
using Microsoft.AspNetCore.Components;

namespace BlazorWebApp.Components.Pages.Masters
{
    public partial class UserClaim
    {
        [SupplyParameterFromForm]
        private UserClaimsModel UserClaimsModel { get; set; } = new();

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [CascadingParameter]
        public HttpContext HttpContext { get; set; } = default!;

        [Inject]
        public IHttpRequestService HttpRequestService { get; set; }

        private async Task OnUserClaimsFormSubmit()
        {
            if (UserClaimsModel != null)
            {
                // need to get the logged in user email from where?
                // we have already implemented cookie authetication and have one cookie called jwtbearertoken. this can be retrieved back and find the claim in it
                // or otherwise we can have another cookie to store the email and retrieve back
                // lets store the email during login and retrieve back
                var email = string.Empty;

                if (HttpContext != null && HttpContext.Request != null)
                {
                    email = HttpContext.Request.Cookies["LoggedInUser"];
                }
                UserClaimsModel.Email = email;
                var request = new ApiRequestModel()
                {
                    ApiType = ApiType.Post,
                    ApiUrl = ApiHelper.AuthWebApiUrl + "api/auth/add/claim",
                    Data = UserClaimsModel
                };
                var response = await HttpRequestService.SendAsync(request, false);
                if (response != null && response.IsSuccess)
                {
                    NavigationManager.NavigateTo("/");
                }
            }
        }
    }
}
