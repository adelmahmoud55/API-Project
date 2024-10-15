using LinkDev.Talabat.Core.Domain.Contracts.Infrastructre;
using LinkDev.Talabat.Core.Domain.Entities.Basket;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Infrastructure.BasketRepository
{
    internal class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;

        public BasketRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
            
        }

        public async Task<CustomerBasket?> GetAsync(string id)
        {
            var basket = await _database.StringGetAsync(id);

            return basket.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(basket!);
        }

        public async Task<CustomerBasket?> UpdateAsync(CustomerBasket basket, TimeSpan timeToLive)
        {
            var updated = await _database.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), timeToLive);

            if(updated) return basket;

            return null;
        }


        public async Task<bool> DeleteAsync(string id) =>  await _database.KeyDeleteAsync(id);



    }
}
