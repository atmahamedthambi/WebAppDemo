﻿namespace AuthWebApi.Models
{
    public class ResponseModel
    {
        public string Message { get; set; } = string.Empty;
        public bool IsSuccess { get; set; } = true;
        public object? Result { get; set; }
    }
}
