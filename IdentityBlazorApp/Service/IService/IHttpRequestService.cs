using IdentityBlazorApp.Models;

namespace IdentityBlazorApp.Service.IService
{
    public interface IHttpRequestService
    {
        Task<ApiResponseModel> SendAsync(ApiRequestModel request);
    }
}
