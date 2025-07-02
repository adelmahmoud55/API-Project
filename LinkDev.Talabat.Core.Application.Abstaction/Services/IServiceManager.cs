using LinkDev.Talabat.Core.Application.Abstaction.Comman.Contracts.Infrastructure;
using LinkDev.Talabat.Core.Application.Abstaction.Services.Auth;
using LinkDev.Talabat.Core.Application.Abstaction.Services.Orders;
using LinkDev.Talabat.Core.Application.Abstaction.Services.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application.Abstaction.Services
{
    public interface IServiceManager
    {
        // readonly properties for each Application service interface
        public IOrderService   OrderService { get;  }
        public IProductService ProductService { get; }

        public IAuthService AuthService { get; }
    }
}
