using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BlazorWebApp.Models
{
    public class UserRegistrationModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        public string Password { get; set; }

        public string? Role { get; set; }

        // Load all the roles 

        [JsonIgnore]
        public List<RolesModel> Roles { get; set; } = new(); // this is the model we are passing as a payload for api endpoint and there we don't have this roles
        // so we can ignore passign this roles to the payload or otherwise we don't need to specify this Roles in this model

    }
}
