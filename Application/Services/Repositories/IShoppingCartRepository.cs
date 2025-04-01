using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Repositories
{
    public interface IShoppingCartRepository
    {
        Task<ShoppingCart> GetCartAsync(string userId);
        Task SaveCartAsync(ShoppingCart cart);
    }
}
