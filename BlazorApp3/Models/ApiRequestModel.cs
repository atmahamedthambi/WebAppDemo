namespace BlazorApp3.Models
{
    public class ApiRequestModel
    {
        public string ApiUrl { get; set; }
        public object Data { get; set; }

        public ApiType ApiType { get; set; }
    }

    public enum ApiType
    {
        Get, Post, Put, Delete
    }
}
