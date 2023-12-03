using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;

namespace Talabat.Repository.Repository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;

        public BasketRepository(IConnectionMultiplexer Redis)
        {
            _database = Redis.GetDatabase();
        }
        public async Task<CustomerBasket?> GetBasketAsync(string BasketId)
        {
            var basket = await _database.StringGetAsync(BasketId);
            return basket.IsNull ? null : JsonSerializer.Deserialize<CustomerBasket>(basket);
        }

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
        {
            var CreateOrUpdatedBasket = await _database.StringSetAsync(basket.Id,
                 JsonSerializer.Serialize(basket), TimeSpan.FromDays(1));
            if (CreateOrUpdatedBasket is false) return null;
            return await GetBasketAsync(basket.Id);
        }
        public Task<bool> DeleteBasketAsync(string BasketId)
        {
            return _database.KeyDeleteAsync(BasketId);
        }
    }
}
