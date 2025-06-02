using Application.Services.Externals;
using Domain.Entities;
using Infrastructure.EmailManagers;
using Microsoft.Extensions.Options;
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
        private readonly CartSettings _cartSettings;

        public CartService(IConnectionMultiplexer redis, IOptions<CartSettings> cartSettings)
        {
            _redisDb = redis.GetDatabase();
            _cartSettings = cartSettings.Value;
        }

        private string GetCartKey(string userId) => $"cart:{userId}";

        public async Task<Cart?> GetCartAsync(string userId)
        {
            var data = await _redisDb.StringGetAsync(GetCartKey(userId));
            return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<Cart>(data!);
        }

        public async Task SaveCartAsync(Cart cart)
        {
            var serialized = JsonSerializer.Serialize(cart);
            await _redisDb.StringSetAsync(GetCartKey(cart.UserId), serialized, TimeSpan.FromDays(_cartSettings.CartExpirationDays));
        }

        public async Task DeleteCartAsync(string userId)
        {
            await _redisDb.KeyDeleteAsync(GetCartKey(userId));
        }
    }
}
