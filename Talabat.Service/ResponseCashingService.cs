using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Service_Contract;

namespace Talabat.Service
{
    public class ResponseCashingService : IResponseCashingService
    {
        private readonly IDatabase _database;
        public ResponseCashingService(IConnectionMultiplexer Redis) 
        {
            _database = Redis.GetDatabase();
        }

        public async Task CashResponseAsync(string cashKey, object Response, TimeSpan ExpiredTime)
        {
            if (Response is null) return;

            var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            var serializedResponse = JsonSerializer.Serialize(Response,options);

            await _database.StringSetAsync(cashKey, serializedResponse , ExpiredTime);
        }

        public async Task<string?> GetCashedResponseAsync(string cashKey)
        {
            var response = await _database.StringGetAsync(cashKey);
            if (response.IsNullOrEmpty) return null;
            return response;
        }
    }
}
