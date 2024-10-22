using AutoMapper;
using LinkDev.Talabat.Core.Application.Abstaction;
using LinkDev.Talabat.Core.Application.Abstaction.Basket;
using LinkDev.Talabat.Core.Application.Basket;
using LinkDev.Talabat.Core.Application.Products;
using LinkDev.Talabat.Core.Application.Services.Products;
using LinkDev.Talabat.Core.Domain.Contracts.Infrastructre;
using LinkDev.Talabat.Core.Domain.Contracts.Persistence;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application.Services
{
    internal class ServiceManager : IServiceManager
    {

        private readonly Lazy<IProductService> _productService;
        private readonly Lazy<IBasketService> _basketService;


        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public ServiceManager(IUnitOfWork unitOfWork, IMapper mapper,IConfiguration configuration,Func<IBasketService> basketServiceFactory)
        {

            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;


            _productService = new Lazy<IProductService>(() => new ProductService(_unitOfWork, _mapper)); //no need to register the ProductService in DI container because it is created here.

            _basketService = new Lazy<IBasketService>(basketServiceFactory); //no need to register the BasketService in DI container because it is created here.
           
        }


        public IProductService ProductService => _productService.Value;

        public IBasketService BasketService => _basketService.Value;

        #region Lazy initialization
        /*
         * Yes, when we access the ProductService property, the corresponding object will be created.In this code, the ProductService property is defined as a getter-only property of type IProductService.It uses lazy initialization to create the object only when it is accessed for the first time.
        The ProductService property is backed by a Lazy<IProductService> field named _productService.The Lazy<T> class is used to defer the creation of an object until it is actually needed.In this case, the Lazy<IProductService> field is initialized with a lambda expression that creates a new instance of the ProductService class, passing in the unitOfWork and mapper dependencies.
    When the ProductService property is accessed, the Value property of the Lazy<IProductService> field is called.If the object has not been created yet, the Value property will create the object by invoking the lambda expression. Once the object is created, subsequent accesses to the ProductService property will return the same instance without creating a new one.
    This lazy initialization approach can be useful in scenarios where creating the object is expensive or requires certain dependencies.It ensures that the object is only created when it is actually needed, improving performance and resource utilization.*





         */
        #endregion
    }
}