
using BlazorApp3.Models;

namespace BlazorApp3.Service.IService
{
    public interface IHttpRequestService
    {
        Task<ApiResponseModel> SendAsync(ApiRequestModel request);
    }
}
