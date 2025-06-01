using Application.Features.Accounts.Events;
using Application.Features.Orders.Events;
using Application.Services.CQS.Commands;
using Application.Services.Externals;
using Application.Services.Repositories;
using Domain.Constants;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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
    public class CartDto
    {
        public string ProductVariantId { get; set; }
        public string ProductName { get; set; }
        public string SizeName { get; set; }
        public string ColorName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
    }
    public class CreateOrderResult
    {
        public string Id { get; init; } = null!;
        public string Message { get; init; } = null!;
        public string? PaymentUrl { get; init; } 
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
        private readonly IMediator _mediator;
        private readonly IIdentityService _identityService;

        public CreateOrderHandler(
            IBaseCommandRepository<Order> repository,
            IBaseCommandRepository<ProductVariant> repositoryVariant,
            ICommandContext context,
            IUnitOfWork unitOfWork,
            ICommonService commonService,
            IVnPayService vnPayService,
            IMediator mediator,
            IIdentityService identityService
            )
        {
            _repository = repository;
            _repositoryVariant = repositoryVariant;
            _context = context;
            _unitOfWork = unitOfWork;
            _commonService = commonService;
            _vnPayService = vnPayService;
            _mediator = mediator;
            _identityService = identityService;
        }

        public async Task<CreateOrderResult> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            var orderCode = _commonService.GenerateCode("DH");
            var totalAmount = request.Items.Sum(x => x.Quantity * x.Price);
            var totalQuantity = request.Items.Sum(x => x.Quantity);
            decimal totalDiscount = 0;
            if (request.DiscountValue > 0)
            {
                if (request.DiscountType == 0)
                {
                    totalDiscount = totalAmount * request.DiscountValue / 100;
                    totalAmount -= totalDiscount;
                }
                else if (request.DiscountType == 1)
                {
                    totalDiscount = request.DiscountValue;
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

            var status = OrderStatus.Pending;
            var statusPayment = StatusPayment.NotPaid;
            var order = new Order(
                request.CustomerId,
                orderCode,
                totalAmount,
                totalDiscount,
                totalQuantity,
                request.TypePayment,
                status,
                statusPayment,
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

            // Send email for customer
            var variantIds = request.Items.Select(i => i.ProductVariantId).ToList();

            var variants = await _context.ProductVariant
                .Include(v => v.Product)
                .Include(v => v.Color)
                .Include(v => v.Size)
                .Where(v => variantIds.Contains(v.Id))
                .ToListAsync();

            var newCartDto = request.Items.Select(i =>
            {
                var variant = variants.FirstOrDefault(v => v.Id == i.ProductVariantId);

                return new CartDto
                {
                    ProductVariantId = i.ProductVariantId,
                    ProductName = variant?.Product?.Title ?? "N/A",
                    ColorName = variant?.Color?.Name ?? "N/A",
                    SizeName = variant?.Size?.Name ?? "N/A",
                    Price = i.Price,
                    Quantity = i.Quantity,
                    TotalPrice = i.Price * i.Quantity
                };
            }).ToList();
            var user = await _identityService.GetUserByIdAsync(request.CustomerId);
            var addressOrder = _context.ShippingAddress.FirstOrDefault(x => x.Id == request.ShippingAddressId);
            var registerUserEvent = new SendMailOrderEvent
            (
                newCartDto,
                order,
                addressOrder,
                user.Email,
                totalDiscount
            );
            await _mediator.Publish(registerUserEvent);

            string? paymentUrl = null;
            if (request.TypePayment == 2)
            {
                paymentUrl = _vnPayService.CreatePaymentUrl(order, request.TypePayment);
            }


            return new CreateOrderResult
            {
                Id = order.Code,
                Message = "Success",
                PaymentUrl = paymentUrl
            };
        }
    }
}
