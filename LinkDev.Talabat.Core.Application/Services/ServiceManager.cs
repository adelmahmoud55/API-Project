using AutoMapper;
using LinkDev.Talabat.Core.Application.Abstaction;
using LinkDev.Talabat.Core.Application.Products;
using LinkDev.Talabat.Core.Application.Services.Products;
using LinkDev.Talabat.Core.Domain.Contracts.Persistence;
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

        public ServiceManager(IUnitOfWork unitOfWork,IMapper mapper) 
        {
           _productService = new Lazy<IProductService>(() => new ProductService(unitOfWork, mapper)); //no need to register the ProductService in DI container because it is created here.
        }
        public IProductService ProductService => _productService.Value;
    }

    #region Lazy initialization
    /*
     * Yes, when we access the ProductService property, the corresponding object will be created.In this code, the ProductService property is defined as a getter-only property of type IProductService.It uses lazy initialization to create the object only when it is accessed for the first time.
    The ProductService property is backed by a Lazy<IProductService> field named _productService.The Lazy<T> class is used to defer the creation of an object until it is actually needed.In this case, the Lazy<IProductService> field is initialized with a lambda expression that creates a new instance of the ProductService class, passing in the unitOfWork and mapper dependencies.
When the ProductService property is accessed, the Value property of the Lazy<IProductService> field is called.If the object has not been created yet, the Value property will create the object by invoking the lambda expression. Once the object is created, subsequent accesses to the ProductService property will return the same instance without creating a new one.
This lazy initialization approach can be useful in scenarios where creating the object is expensive or requires certain dependencies.It ensures that the object is only created when it is actually needed, improving performance and resource utilization.*
     
     
     servie manager hwa ms2ol 3n kol el services f ana lma bro7 a3ml object mno hwa gwa prop lkol service , f lw enta sh8al  Immediate Initialization y3ni mn 8er lazy
    once enk create el service manger object hyb2a 3ndk objects l kol el services { product , order , user , ...etc } ,w momkn akon msh m7tag eni ast5dmhom kolhom 
    lakn with lazy initialziation hyb2a 3ndk object l service ely enta 3ayzha bs lma t3ml access 3leha
    bm3na enta create el object mn service manger , ay service gwah hy7slha object creation lma enta t2ol , service manager.ProductService 3mlt access leha
    f hwa by3ml deferred for creating the object until it is actually needed ,
    w dh kwis mn n7yt : service may not always be required.
                         You want to optimize resource usage.
                         You have complex initialization logic that can be deferred. 
     
     
     */
    #endregion
}
