using Application.Services.CQS.Commands;
using Application.Services.Repositories;
using Domain.Constants;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Orders.Commands
{
    public class CancelOrderResult
    {
        public string Message { get; init; } = null!;
    }

    public class CancelOrderRequest : IRequest<CancelOrderResult>
    {
        public string OrderId { get; set; } = null!;
    }

    public class CancelOrderValidator : AbstractValidator<CancelOrderRequest>
    {
        public CancelOrderValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty();
        }
    }


    public class CancelOrderHandler : IRequestHandler<CancelOrderRequest, CancelOrderResult>
    {
        private readonly IBaseCommandRepository<Order> _orderRepository;
        private readonly IBaseCommandRepository<ProductVariant> _productVariantRepository;
        private readonly ICommandContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public CancelOrderHandler(
            IBaseCommandRepository<Order> orderRepository,
            IBaseCommandRepository<ProductVariant> productVariantRepository,
            ICommandContext context,
            IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _productVariantRepository = productVariantRepository;
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<CancelOrderResult> Handle(CancelOrderRequest request, CancellationToken cancellationToken)
        {
            // Tìm đơn hàng
            var order = await _context.Order
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

            if (order == null)
            {
                throw new ApplicationException($"{ExceptionConsts.EntitiyNotFound}");
            }

            if (order.Status != OrderStatus.Pending)
            {
                throw new ApplicationException("Đơn hàng không thể hủy ở trạng thái hiện tại.");
            }

            order.Status = OrderStatus.Cancelled; 
            _orderRepository.Update(order);

            // Cập nhật lại tồn kho sản phẩm (trả lại số lượng)
            foreach (var detail in order.OrderDetails)
            {
                var variant = await _productVariantRepository.GetByIdAsync(detail.ProductVariantId, cancellationToken);
                if (variant != null)
                {
                    variant.Quantity += detail.Quantity;
                    _productVariantRepository.Update(variant);
                }
            }

            await _unitOfWork.SaveAsync(cancellationToken);

            return new CancelOrderResult
            {
                Message = "Order cancelled successfully."
            };
        }
    }
}
