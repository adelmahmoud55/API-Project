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
        public ProductWithBrandAndCategorySpecifications(string? sort) : base() // to return all products
        {
            AddIncludes();

            AddOrderBy(p => p.Name); // to sort the products by name

            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "nameDesc":
                        AddOrderByDesc(p => p.Name);/* OrderBy = p => p.Name;*/
                        break;

                    case "priceAsc":
                        AddOrderBy(p => p.Price);
                        break;

                    case "priceDesc":
                        AddOrderByDesc(p => p.Price);
                        break;

                    default:
                        AddOrderBy(p => p.Name);
                        break;
                }
            }



        }




        // this object is created via this constructor ,will be used for building the query that will be get specific product
        public ProductWithBrandAndCategorySpecifications(int id) : base(id) // to return specific product
        {
            AddIncludes();
        }


        private protected override void AddIncludes()
        {
            base.AddIncludes();

            Includes.Add(p => p.Brand!);
            Includes.Add(p => p.Category!);

        }
    }

}

