using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Externals
{
    public interface ICartService
    {
        Task DeleteCartAsync(string userId);
        Task<ShoppingCart?> GetCartAsync(string userId);
        Task SaveCartAsync(ShoppingCart cart);
    }
}
