using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Domain.Entities.Products
{
    public class Product : BaseAuditableEntity<int>
    {
        public required string Name { get; set; }
        public required string NormalizedName { get; set; }

        public required string Description { get; set; }

        public string? PictureUrl { get; set; }

        public decimal Price { get; set; }


        public int? BrandId { get; set; } // Foriegn Key --> ProductBrand Entity. {? means Optional}
        public virtual ProductBrand? Brand { get; set; } // null cuz when delete the brand the product still exist, and BrandId is set to null.


        public int? CategoryId { get; set; } // Foriegn Key --> ProductCategory Entity. {? means Optional}
        public virtual ProductCategory? Category { get; set; } // null cuz when delete the category the product still exist, and CategoryId is set to null.

    }
}
