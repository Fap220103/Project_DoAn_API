using Application.Services.CQS.Commands;
using Application.Services.Externals;
using Application.Services.Repositories;
using Domain.Constants;
using Domain.Entities;
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
        public string ShippingAddressId { get; set; } = null!;

        public List<OrderItemDto> Items { get; set; } = new();
        public string DiscountId { get; set; }
        public decimal DiscountValue { get; set; } = 0;
        public int DiscountType { get; set; } = 0;
    }

    public class CreateOrderValidator : AbstractValidator<CreateOrderRequest>
    {
        public CreateOrderValidator()
        {
            RuleFor(x => x.CustomerId)
                  .NotEmpty();
            RuleFor(x => x.ShippingAddressId)
              .NotEmpty();
        }
    }


    public class CreateOrderHandler : IRequestHandler<CreateOrderRequest, CreateOrderResult>
    {
        private readonly IBaseCommandRepository<Order> _repository;
        private readonly IBaseCommandRepository<ProductVariant> _repositoryVariant;
        private readonly ICommandContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICommonService _commonService;
        private readonly IVnPayService _vnPayService;

        public CreateOrderHandler(
            IBaseCommandRepository<Order> repository,
            IBaseCommandRepository<ProductVariant> repositoryVariant,
            ICommandContext context,
            IUnitOfWork unitOfWork,
            ICommonService commonService,
            IVnPayService vnPayService
            )
        {
            _repository = repository;
            _repositoryVariant = repositoryVariant;
            _context = context;
            _unitOfWork = unitOfWork;
            _commonService = commonService;
            _vnPayService = vnPayService;
        }

        public async Task<CreateOrderResult> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            var orderCode = _commonService.GenerateCode("DH");
            var totalAmount = request.Items.Sum(x => x.Quantity * x.Price);
            var totalQuantity = request.Items.Sum(x => x.Quantity);

            if (request.DiscountValue > 0)
            {
                if (request.DiscountType == 0)
                {
                    totalAmount -= totalAmount * request.DiscountValue / 100;
                }
                else if (request.DiscountType == 1)
                {
                    totalAmount -= request.DiscountValue;
                }

                totalAmount = Math.Max(totalAmount, 0);

                // xử lý số lượng discount
                var userDiscountQuery = await _context.UserDiscount
                                          .FirstOrDefaultAsync(x => x.UserId == request.CustomerId && x.DiscountId == request.DiscountId);

                if (userDiscountQuery != null)
                {
                    userDiscountQuery.IsUsed = true;
                    _context.UserDiscount.Update(userDiscountQuery);
                }
                var discountQuery = await _context.Discount
              .FirstOrDefaultAsync(x => x.Id == request.DiscountId);

                if (discountQuery != null)
                {
                    discountQuery.UsedCount++;
                    _context.Discount.Update(discountQuery);
                }
                _context.Discount.Update(discountQuery);

                await _context.SaveChangesAsync();
            }

            var status = 1;
            var order = new Order(
                request.CustomerId,
                orderCode,
                totalAmount,
                totalQuantity,
                request.TypePayment,
                status,
                request.ShippingAddressId
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
                        throw new ApplicationException($"Không đủ số lượng sản phẩm {variant.Id} trong kho.");
                    }
                    else
                    {
                        variant.Quantity -= item.Quantity;
                        _repositoryVariant.Update(variant);
                    }
                }
                else
                {
                    throw new ApplicationException($"{ExceptionConsts.EntitiyNotFound}");
                }
            }

            await _unitOfWork.SaveAsync(cancellationToken);
            if (request.TypePayment == 2)
            {
                _vnPayService.UrlPayment(request.TypePayment, orderCode);
            }


            return new CreateOrderResult
            {
                Id = order.Id,
                Message = "Success"
            };
        }
    }
}
