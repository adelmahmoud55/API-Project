using System.ComponentModel.DataAnnotations;

namespace LinkDev.Talabat.Dashboard.Models
{
    public class RoleFormViewModel
    {
        [Required(ErrorMessage = "Role Name is required.")]
        [StringLength(256, ErrorMessage = "Role Name cannot be longer than 50 characters.")]
        public string Name { get; set; } 
    }
}
