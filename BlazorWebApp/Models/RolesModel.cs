using System.ComponentModel.DataAnnotations;

namespace BlazorWebApp.Models
{
    public class RolesModel
    {
        [Required]
        public string Name { get; set; }
    }
}
