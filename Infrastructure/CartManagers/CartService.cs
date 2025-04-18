using Application.Services.Externals;
using Domain.Entities;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace Infrastructure.CartManagers
{
    public class CartService : ICartService
    {
        private readonly IDatabase _redisDb;

        public CartService(IConnectionMultiplexer redis)
        {
            _redisDb = redis.GetDatabase();
        }

        private string GetCartKey(string userId) => $"cart:{userId}";

        public async Task<ShoppingCart?> GetCartAsync(string userId)
        {
            var data = await _redisDb.StringGetAsync(GetCartKey(userId));
            return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<ShoppingCart>(data!);
        }

        public async Task SaveCartAsync(ShoppingCart cart)
        {
            var serialized = JsonSerializer.Serialize(cart);
            await _redisDb.StringSetAsync(GetCartKey(cart.UserId), serialized);
        }

        public async Task DeleteCartAsync(string userId)
        {
            await _redisDb.KeyDeleteAsync(GetCartKey(userId));
        }
    }
}
