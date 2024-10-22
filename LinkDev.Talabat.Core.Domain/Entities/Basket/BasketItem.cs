using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Domain.Entities.Basket
{
    // by contract between backend and frontend
    public class BasketItem
    {
       
        public int Id { get; set; } //same Id for the product

        public required string ProductName { get; set; }

        public string? PictureUrl { get; set; }

        public decimal price { get; set; }

        public int Quantity { get; set; }

        public string? Brand { get; set; }

        public string? Category { get; set; }






    }
}
