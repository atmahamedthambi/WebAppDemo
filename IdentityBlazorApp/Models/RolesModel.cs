using System.ComponentModel.DataAnnotations;

namespace IdentityBlazorApp.Models
{
    public class RolesModel
    {
        [Required]
        public string Name { get; set; }
    }
}
