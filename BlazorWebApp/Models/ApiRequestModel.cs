namespace BlazorWebApp.Models
{
    public class ApiRequestModel
    {
        public string ApiUrl { get; set; }
        public object Data { get; set; }
        public ApiType ApiType { get; set; }
        public string Token { get; set; }
    }

    public enum ApiType
    {
        Get, Post, Put, Delete
    }
}
