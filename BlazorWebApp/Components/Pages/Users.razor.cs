using BlazorWebApp.Models;
using BlazorWebApp.Service.IService;
using BlazorWebApp.Utility;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace BlazorWebApp.Components.Pages
{
    public partial class Users
    {
        [Inject]
        public IHttpRequestService HttpRequestService { get; set; }

        [CascadingParameter]
        public HttpContext HttpContext { get; set; } = default!;

        private List<UserModel> UserList { get; set; } = new();

        protected async override Task OnInitializedAsync()
        {
            var token = string.Empty;
            if (HttpContext != null && HttpContext.Request.Cookies != null)
            {
                // retrieving cookies should be used from Request
                token = HttpContext.Request.Cookies["JwtBearerToken"];
            }
            // here we need to call the end point
            var request = new ApiRequestModel()
            {
                ApiType = ApiType.Get,
                ApiUrl = ApiHelper.AuthWebApiUrl + "api/user/allusers",
                Token = token
            };
            var response = await HttpRequestService.SendAsync(request, true);
            if(response != null && response.IsSuccess)
            {
                var result = JsonConvert.DeserializeObject<List<UserModel>>(Convert.ToString(response.Result));
                if(result != null && result.Any())
                {
                    UserList = result.ToList();
                }
            }
        }
    }
}
