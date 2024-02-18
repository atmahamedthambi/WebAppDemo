using BlazorApp3.Models;
using BlazorApp3.Service.IService;
using Newtonsoft.Json;
using System.Text;

namespace BlazorApp3.Service
{
    public class HttpRequestService : IHttpRequestService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpRequestService(IHttpClientFactory httpClientFactory)
        {
            this._httpClientFactory = httpClientFactory;
        }
        public async Task<ApiResponseModel> SendAsync(ApiRequestModel request)
        {
            try
            {
                HttpClient httpClient = _httpClientFactory.CreateClient("AuthApi");
                HttpRequestMessage message = new();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(request.ApiUrl);
                if (request.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(request.Data), Encoding.UTF8, "application/json");
                }
                switch (request.ApiType)
                {
                    case ApiType.Get:
                        message.Method = HttpMethod.Get; break;
                    case ApiType.Post:
                        message.Method = HttpMethod.Post; break;
                    case ApiType.Put:
                        message.Method = HttpMethod.Put; break;
                    case ApiType.Delete:
                        message.Method = HttpMethod.Delete; break;
                }
                var apiResponse = await httpClient.SendAsync(message);
                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                var apiResponseModel = JsonConvert.DeserializeObject<ApiResponseModel>(apiContent);
                return apiResponseModel;

            }
            catch (Exception ex)
            {
                return new ApiResponseModel()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
    }
}
