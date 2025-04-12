using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.Inventories
{
    public class InventoryController : BaseApiController
    {
        public InventoryController(ISender sender) : base(sender)
        {
        }
    }
}
