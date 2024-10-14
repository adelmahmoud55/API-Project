using LinkDev.Talabat.Core.Application.Abstaction.Comman;
using LinkDev.Talabat.Core.Application.Abstaction.Products;
using LinkDev.Talabat.Core.Application.Products.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application.Products
{
    public interface IProductService
    {
        Task<Pagination<ProductToReturnDto>> GetProductsAsync(ProductSepcParams SepcParams);

        Task<ProductToReturnDto> GetProductAsync(int id);

        Task<IEnumerable<BrandDto>> GetBrandsAsync();

        Task<IEnumerable<CategoryDto>> GetCategoriesAsync();
    }
}
