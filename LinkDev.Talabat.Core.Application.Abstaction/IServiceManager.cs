using LinkDev.Talabat.Core.Application.Abstaction.Basket;
using LinkDev.Talabat.Core.Application.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application.Abstaction
{
    public interface IServiceManager
    {
        public IProductService ProductService { get; }

        public IBasketService  BasketService { get;  }
    }
}
