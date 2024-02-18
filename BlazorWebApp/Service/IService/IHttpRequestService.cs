using BlazorWebApp.Models;

namespace BlazorWebApp.Service.IService
{
    public interface IHttpRequestService
    {
        Task<ApiResponseModel> SendAsync(ApiRequestModel request, bool includeToken);
    }
}
