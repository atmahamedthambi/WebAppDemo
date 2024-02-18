using BlazorWebApp.Models;
using BlazorWebApp.Service.IService;
using BlazorWebApp.Utility;
using Microsoft.AspNetCore.Components;

namespace BlazorWebApp.Components.Pages.Masters
{
    public partial class Roles
    {
        [SupplyParameterFromForm]
        public RolesModel RolesModel { get; set; } = new();

        [Inject]
        public IHttpRequestService HttpRequestService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        private async Task OnRolesFormSubmit()
        {
            if(RolesModel != null)
            {
                var request = new ApiRequestModel()
                {
                    ApiType = ApiType.Post,
                    ApiUrl = ApiHelper.AuthWebApiUrl + "api/auth/add/role",
                    Data = RolesModel.Name
                };
                var response = await HttpRequestService.SendAsync(request, false);
                if(response != null && response.IsSuccess)
                {
                    NavigationManager.NavigateTo("/");
                }
            }
        }
    }
}
