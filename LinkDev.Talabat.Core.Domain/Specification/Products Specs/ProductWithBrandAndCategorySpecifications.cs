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
        public ProductWithBrandAndCategorySpecifications(string? sort, int? brandId, int? categoryId, int pageSize ,int PageIndex) :
            base(

                   P =>
                          (!brandId.HasValue || P.BrandId == brandId.Value)

                                             &&

                          (!categoryId.HasValue || P.CategoryId == categoryId.Value)


                ) // to return all products
        {


            AddIncludes();


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

            /// we make the pagination after filtering and sorting
            /// (skip ,take)
            /// total products = 18 ,
            /// page size = 5
            /// page index = 3
            ///paging get to the closest number that can divide the page index = 20/5 = 4 , so we will have 4 pages ,every page contain 5 items, and the last page will have 3 items
            /// skip = (3-1) * 5 = 10

            AddPagination(pageSize * (PageIndex - 1)  , pageSize);


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

