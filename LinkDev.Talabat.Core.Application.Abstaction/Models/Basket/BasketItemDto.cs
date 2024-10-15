using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application.Abstaction.Models.Basket
{
    public class BasketItemDto
    {

        [Required]
        public int Id { get; set; } //same Id for the product

        [Required(ErrorMessage = "Product Name is Required")]
        public required string ProductName { get; set; }

        public string? PictureUrl { get; set; }

        [Required]
        [Range(.1, double.MaxValue, ErrorMessage = "Price must be greater than Zero")]
        public decimal price { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least one Item")]
        public int Quantity { get; set; }

        public string? Brand { get; set; }

        public string? Category { get; set; }

    }
}
