﻿namespace BlazorApp3.Models
{
    public class UserRegistrationModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string? Role { get; set; }
    }
}
