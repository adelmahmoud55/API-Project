using System.ComponentModel.DataAnnotations;

namespace LinkDev.Talabat.Dashboard.Models
{
    public class ProductBrandFormViewModel
    {
        [Required(ErrorMessage = "Please enter a new name")]
        public string Name { get; set; }
    }
}
