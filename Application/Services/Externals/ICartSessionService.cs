using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Externals
{
    public interface ICartSessionService
    {
        ShoppingCart GetCart();
        void SetCart(ShoppingCart cart);
    }
}
