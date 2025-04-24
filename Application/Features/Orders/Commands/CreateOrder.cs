using Application.Services.Externals;
using Application.Services.Repositories;
using Domain.Constants;
using Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Orders.Commands
{
    public class OrderItemDto
    {
        public string ProductVariantId { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public class CreateOrderResult
    {
        public string Id { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class CreateOrderRequest : IRequest<CreateOrderResult>
    {
        public string CustomerId { get; set; } = null!;
        public int TypePayment { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
    }

    public class CreateOrderValidator : AbstractValidator<CreateOrderRequest>
    {
        public CreateOrderValidator()
        {
        
        }
    }


    public class CreateOrderHandler : IRequestHandler<CreateOrderRequest, CreateOrderResult>
    {
        private readonly IBaseCommandRepository<Order> _repository;
        private readonly IBaseCommandRepository<ProductVariant> _repositoryVariant;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICommonService _commonService;
        private readonly IVnPayService _vnPayService;

        public CreateOrderHandler(
            IBaseCommandRepository<Order> repository,
             IBaseCommandRepository<ProductVariant> repositoryVariant,
            IUnitOfWork unitOfWork,
            ICommonService commonService,
            IVnPayService vnPayService
            )
        {
            _repository = repository;
            _repositoryVariant = repositoryVariant;
            _unitOfWork = unitOfWork;
            _commonService = commonService;
            _vnPayService = vnPayService;
        }

        public async Task<CreateOrderResult> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            var orderCode = _commonService.GenerateCode("DH");
            var totalAmount = request.Items.Sum(x => x.Quantity * x.Price);
            var totalQuantity = request.Items.Sum(x => x.Quantity);
            var status = 1;
            var order = new Order(
                request.CustomerId,
                orderCode,
                totalAmount,
                totalQuantity,
                request.TypePayment,
                status
            );
            order.OrderDetails = request.Items.Select(i => new OrderDetail
            {
                Id = Guid.NewGuid().ToString(),
                ProductVariantId = i.ProductVariantId,
                Quantity = i.Quantity,
                Price = i.Price,
                OrderId = order.Id,
            }).ToList();

            await _repository.CreateAsync(order);

            foreach (var item in request.Items)
            {
                var variant = await _repositoryVariant.GetByIdAsync(item.ProductVariantId, cancellationToken);
                if (variant != null)
                {
                    if (variant.Quantity < item.Quantity)
                    {
                        // Nếu không đủ số lượng, trả về lỗi hoặc thông báo
                        throw new ApplicationException($"Không đủ số lượng sản phẩm {variant.Id} trong kho.");
                    }
                    else
                    {
                        // Trừ số lượng tồn kho
                        variant.Quantity -= item.Quantity;
                        _repositoryVariant.Update(variant);  // Cập nhật lại sản phẩm vào cơ sở dữ liệu
                    }
                }
                else
                {
                    // Nếu không tìm thấy sản phẩm, có thể thông báo lỗi hoặc xử lý thêm.
                    throw new ApplicationException($"{ExceptionConsts.EntitiyNotFound}");  
                }
            }

            await _unitOfWork.SaveAsync(cancellationToken);
            if (request.TypePayment == 2)
            {
                _vnPayService.UrlPayment(request.TypePayment,orderCode);
            }

            return new CreateOrderResult
            {
                Id = order.Id,
                Message = "Success"
            };
        }
    }
}
