using LinkDev.Talabat.Core.Domain.Contracts;
using LinkDev.Talabat.Infrastructure.Persistence.Data;
using LinkDev.Talabat.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Infrastructure.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _dbContext;
        //private readonly Dictionary<string, object> _repositories;
        private readonly ConcurrentDictionary<string, object> _repositories; // thread safe dictionary , work with async programming 



        public UnitOfWork(StoreContext dbContext)
        {
            _dbContext = dbContext;
            //_repositories = new Dictionary<string, object>();
            _repositories = new ();
            //_repositories = [];


        }

        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>()
           where TEntity : BaseEntity<TKey>
           where TKey : IEquatable<TKey>
        {
            // work with doctionary , but with Async programming we use ConcurrentDictionary {thread safe} in case of multiple threads {Concurrent DataStructure } 

            /// var typeName = typeof(TEntity).Name; // Product
            /// if (_repositories.ContainsKey(typeName)) return (IGenericRepository<TEntity, TKey>) _repositories[typeName]; // cast object to the generic repository
            ///
            ///
            /// var repsository = new GenericRepository<TEntity, TKey>(_dbContext);
            ///
            /// _repositories.Add(typeName, repsository);
            /// return repsository; 

            _repositories.GetOrAdd(typeof(TEntity).Name, new GenericRepository<TEntity, TKey>(_dbContext));


            return (IGenericRepository<TEntity, TKey>)_repositories.GetOrAdd(typeof(TEntity).Name, new GenericRepository<TEntity, TKey>(_dbContext)); //This approach ensures that only one repository object is created per entity type, even if multiple threads are accessing the GetRepository method concurrently. The concurrent dictionary provides thread-safe access and modification of the dictionary, preventing any race conditions or conflicts

        }



        public async Task<int> CompleteAsync() => await _dbContext.SaveChangesAsync();


        public async ValueTask DisposeAsync() => await _dbContext.DisposeAsync();




        #region Concuurent DataStructure and Thread Safety 
        /*
         In the provided code, the UnitOfWork class is responsible for managing the repositories and providing access to them through the GetRepository method.
        The GetRepository method returns a repository object based on the entity type and key type provided as generic parameters.
         When you access the GetRepository method multiple times within a service or across different methods, it will return the same repository object for the same entity type. 
        This means that if you call GetRepository multiple times for the same entity type within a service or across different methods, it will return the same repository object instance.
         The ConcurrentDictionary _repositories is used to store and retrieve the repository objects. 
        It ensures that only one repository object is created per entity type, even if multiple threads are accessing the GetRepository method concurrently.
        The concurrent dictionary provides thread-safe access and modification, preventing any race conditions or conflicts.
         So, if you access the GetRepository method multiple times within a service or across different methods, it will return the same repository object instance for the same entity type. 
        This allows you to work with the same repository object and maintain consistency in your data operations.
         It's important to note that the UnitOfWork itself is not limited to a specific method or a single instance. 
        It can be used across multiple methods or even across different services. Each time you create an instance of the UnitOfWork, it will have its own set of repository objects stored in the _repositories dictionary.
         Additionally, the fact that the methods in the UnitOfWork class are marked as async does not affect the behavior of the GetRepository method or the creation of repository objects.
        The async keyword is used to indicate that the methods can be awaited and executed asynchronously, but it does not impact the creation or retrieval of repository objects.
         In summary, when you access the GetRepository method multiple times within a service or across different methods, it will return the same repository object instance for the same entity type.
        This ensures consistency in data operations and allows you to work with the same repository object throughout your service or methods.
         
         */

        #endregion


    }

}

