using LinkDev.Talabat.Core.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Domain.Specification.Products_Specs
{
    public class ProductWithFilterationForCountSpecifications : BaseSpecification<Product, int>
    {
        public ProductWithFilterationForCountSpecifications(int? brandId, int? categoryId, string? search) :

            base(

                   P =>
                              // u can search by name, brand or category 
                            (string.IsNullOrEmpty(search) || P.NormalizedName.Contains(search))  // this condition have to be considered here cuz when u want products with name = mocha , tha count query return the number of products with name = mocha
                                            &&
                            (!brandId.HasValue || P.BrandId == brandId.Value)
                                             &&
                            (!categoryId.HasValue || P.CategoryId == categoryId.Value)
                    
                )
        {

        }


        

    }

}
