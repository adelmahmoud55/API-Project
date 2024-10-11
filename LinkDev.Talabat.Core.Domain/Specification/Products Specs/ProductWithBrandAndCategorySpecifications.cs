using LinkDev.Talabat.Core.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Domain.Specification.Products_Specs
{
    public class ProductWithBrandAndCategorySpecifications : BaseSpecification<Product, int>
    {
        // this object is created via this constructor ,will be used for building the query that will be get all products
        public ProductWithBrandAndCategorySpecifications() : base() // to return all products
        {
            AddIncludes();

        }

      


        // this object is created via this constructor ,will be used for building the query that will be get specific product
        public ProductWithBrandAndCategorySpecifications(int id) : base(id) // to return specific product
        {
            AddIncludes();
        }


        private void AddIncludes()
        {
            Includes.Add(p => p.Brand!);
            Includes.Add(p => p.Category!);
        }
    }

}

