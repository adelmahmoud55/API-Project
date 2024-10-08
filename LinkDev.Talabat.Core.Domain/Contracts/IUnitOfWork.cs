using LinkDev.Talabat.Core.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Domain.Contracts
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        //public IGenericRepository<Product, int> ProductRepsoitory { get; }
        //public IGenericRepository<ProductBrand, int> BrandRepsoitory { get; }
        //public IGenericRepository<ProductCategory, int> CategoryRepsoitory { get; }



        IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() // generic on method level cuz i need it on this method only 
            where TEntity : BaseEntity<TKey>
            where TKey : IEquatable<TKey>;

        Task<int> CompleteAsync();
    }
}
