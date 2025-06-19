using AutoMapper;
using LinkDev.Talabat.Core.Application.Abstaction.Services;
using LinkDev.Talabat.Core.Application.Abstaction.Services.Auth;
using LinkDev.Talabat.Core.Application.Abstaction.Services.Basket;
using LinkDev.Talabat.Core.Application.Abstaction.Services.Products;
using LinkDev.Talabat.Core.Application.Services.Auth;
using LinkDev.Talabat.Core.Application.Services.Basket;
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
        private readonly Lazy<IAuthService> _authService;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        

        public ServiceManager(IUnitOfWork unitOfWork, IMapper mapper,IConfiguration configuration,Func<IBasketService> basketServiceFactory,Func<IAuthService> authServiceFactory)
        {
            // m3 el product service mfesh di container ana pass el dep manaully
            // with auth and basket , We use factory method func 
            // which delegate the creation if the object to the di conatiner where the contaoner 
            // will resolve the dependencies and create the object
            // eli hwa b delegate el creation bta3 el dep lel DI container as long as el 7gat dh already injected
            // lakn m3 el product service , ana 3mlt pass el dep manully ana eli et7kmt 
            // lakn f el complex logic use func delegation , 5li el di container hwa eli y7km f creation el object


            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
                                                //Lambda expression: Automatically converted to a delegate by the compiler.
            _productService = new Lazy<IProductService>(() => new ProductService(_unitOfWork, _mapper)); //no need to register the ProductService in DI container because it is created here.
                                                                                                 // hena bst5dm tre2t el factory methid , func , 3shan el basket service 3ndha dependencies msh mawgoda f el DI container
            _basketService = new Lazy<IBasketService>(basketServiceFactory,LazyThreadSafetyMode.ExecutionAndPublication); //no need to register the BasketService in DI container because it is created here.
            _authService = new Lazy<IAuthService>(authServiceFactory, LazyThreadSafetyMode.ExecutionAndPublication);
        }//hena mst5demn delegate , built in 


        public IProductService ProductService => _productService.Value;

        public IBasketService BasketService => _basketService.Value;

        public IAuthService AuthService => _authService.Value;

        #region Lazy initialization
        /*
         * Yes, when we access the ProductService property, the corresponding object will be created.In this code, the ProductService property is defined as a getter-only property of type IProductService.It uses lazy initialization to create the object only when it is accessed for the first time.
        The ProductService property is backed by a Lazy<IProductService> field named _productService.The Lazy<T> class is used to defer the creation of an object until it is actually needed.In this case, the Lazy<IProductService> field is initialized with a lambda expression that creates a new instance of the ProductService class, passing in the unitOfWork and mapper dependencies.
    When the ProductService property is accessed, the Value property of the Lazy<IProductService> field is called.If the object has not been created yet, the Value property will create the object by invoking the lambda expression. Once the object is created, subsequent accesses to the ProductService property will return the same instance without creating a new one.
    This lazy initialization approach can be useful in scenarios where creating the object is expensive or requires certain dependencies.It ensures that the object is only created when it is actually needed, improving performance and resource utilization.*





         */
        #endregion
        // factory method or func<> bst5dmha lma ykon creation el object depend on objects tanya bs 
        // msh mota7a f e DI container, mn 5lal el factory method a2dr at7km f creation el dependencies eli msh mawgoda f el DI container
        // w hena h7tag eni allow dep injection for the Func <> 
        // 3ks el product service mknsh m7tag ay dependencies 3shan kda msh m3mltlosh fatory method
        // wla e7tft eni allow di leh l2n ana b3mlo creation manually 
        // lakn lma bst5dm factory method lazm allow el DI lel fatory nfcah func


        /* Factory method Vs Direct Intialization
         Great question! The difference between the two approaches you've mentioned comes down to how the object is being created and managed (whether it's through a direct instantiation or a factory method). Let's break it down.

        1. Direct Instantiation Using Lazy<T>
        csharp
        Copy code
        _productService = new Lazy<IProductService>(() => new ProductService(_unitOfWork, _mapper));
        Here, you are directly creating an instance of ProductService inside a Lazy<T>. This approach is called direct instantiation, and you're using Lazy<T> to delay the creation of the ProductService instance until it's actually needed. Here's why this works without a Func:

        Direct Instantiation: In this case, you are creating the ProductService manually by calling its constructor (new ProductService(_unitOfWork, _mapper)).
        Lazy Initialization: The Lazy<IProductService> ensures that the ProductService object is only created when it's first accessed (this is useful for performance, particularly if the instantiation is expensive or might not be needed at all).
        No Func Needed: Since you're manually creating the instance of ProductService in the Lazy<T> constructor, you don't need to use Func<IProductService>. The constructor of Lazy<T> can accept a delegate (() => new ProductService(...)), which directly creates the object when it is first accessed.
        2. Factory Method with Func<T> and Lazy<T>
        csharp
        Copy code
        _basketService = new Lazy<IBasketService>(basketServiceFactory, LazyThreadSafetyMode.ExecutionAndPublication);
        _authService = new Lazy<IAuthService>(authServiceFactory, LazyThreadSafetyMode.ExecutionAndPublication);
        In these cases, you're using a factory method (basketServiceFactory and authServiceFactory) to lazily create instances of IBasketService and IAuthService.

        Factory Method: Instead of directly creating the instance within the Lazy<T> (like with new ProductService(...)), you're passing a Func<T> (a delegate) to Lazy<T>. A Func<T> is essentially a function that returns an instance of the type (IBasketService, IAuthService) when called.
        Factory Logic: The Func<T> (i.e., basketServiceFactory and authServiceFactory) is responsible for creating the service. It is often used when the service depends on complex logic or dependencies that are not registered in the DI container or when you need more control over the instantiation process (e.g., conditionally creating an instance based on some logic).
        Why Func?: The Func<T> allows you to delegate the instantiation of IBasketService and IAuthService to a separate function. This is particularly useful when you have dependencies that cannot be directly resolved from the DI container (like when certain parameters need to be passed to the constructor that are not registered in the container).
        Key Differences Between the Two Approaches:
        Instantiation Process:

        Direct Instantiation (new ProductService(...)):
        You manually create the instance of the service when the Lazy<T> is initialized.
        No Func<T> is needed because you're using a direct constructor call.
        Factory Method (Func<IAuthService> or Func<IBasketService>):
        You delegate the creation of the object to a separate factory function (i.e., basketServiceFactory or authServiceFactory).
        The DI container (or your custom logic) can be used within the factory to resolve the necessary dependencies for creating the object.
        Usage of Dependencies:

        Direct Instantiation: You pass all required dependencies (like _unitOfWork, _mapper) directly into the constructor when creating the object. This is fine when you already have the dependencies available, and the object creation is straightforward.
        Factory Method: The factory method (basketServiceFactory, authServiceFactory) may need to resolve dependencies dynamically or involve complex creation logic. For example, you might need the DI container to resolve certain dependencies, or the service creation logic might require parameters not registered in the DI container.
        DI Container Interaction:

        Direct Instantiation: You're bypassing the DI container entirely by manually creating the object. This means you don't need the DI container to resolve the dependencies for ProductService — you are doing it yourself.
        Factory Method: The factory method (Func<IAuthService>) often involves using the DI container itself (or some other external mechanism) to resolve dependencies that the object needs. This is useful when the dependencies are not registered in the container or if you want to use DI for creating the service dynamically.
        When to Use Each Approach:
        Direct Instantiation with Lazy<T>:

        Use this when you want to control the instantiation of the service manually and you don't need any dynamic or complex logic for creating the object.
        It's useful when the object doesn't have complex dependencies, or you can manually provide the dependencies to the constructor.
        Factory Method with Func<T> and Lazy<T>:

        Use this approach when:
        The object creation is complex and might need to resolve dependencies via the DI container (or perform additional logic).
        You want to delegate the object creation to a separate method, especially if you need conditional logic for object creation.
        The dependencies of the service might not be easily resolvable through the DI container or might change depending on the situation (e.g., using a factory to create a service with different parameters or conditions).
        Summary:
        Lazy<IProductService> with direct instantiation (new ProductService(...)) is a simple, straightforward approach where you manually create the service and pass the required dependencies.

        Lazy<IAuthService> or Lazy<IBasketService> with a factory method (Func<IAuthService>, basketServiceFactory) is used when you want more flexibility in object creation, such as resolving dependencies dynamically, using the DI container, or performing conditional logic during the service's instantiation.

        Both approaches ensure that the object is lazily created, meaning it will only be instantiated when it’s first needed, but the choice of whether to directly instantiate the object or use a factory method depends on the complexity of the dependencies and the flexibility you need.





         */



    }
}