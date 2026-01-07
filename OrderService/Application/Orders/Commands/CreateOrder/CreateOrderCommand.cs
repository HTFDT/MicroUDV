using OrderService.Application.Orders.Commands.CompleteOrder;
using OrderService.Application.Orders.Commands.CreateOrder.DTOs;
using OrderService.Domain.Storage.Abstractions;
using OrderService.Domain.Types;
using Shared.Application.Infrastructure.Cqs;
using Shared.Application.Infrastructure.Messenging.Abstractions;
using Shared.Application.Infrastructure.Results;
using Shared.Application.Orders.Messages.Events;
using Shared.Domain.Types;
using IResult = Shared.Application.Infrastructure.Results.Abstractions.IResult;

namespace OrderService.Application.Orders.Commands.CreateOrder;

public class CreateOrderCommand(CreateOrderDto dto) : Command
{
    public CreateOrderDto Dto { get; set; } = dto;
}

public class CreateOrderCommandHandler : CommandHandler<CreateOrderCommand>
{
    private readonly IOrderRepository _repository;
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<CompleteOrderCommandHandler> _logger;

    public CreateOrderCommandHandler(
        IOrderRepository repository,
        IMessagePublisher publisher,
        ILogger<CompleteOrderCommandHandler> logger)
    {
        _repository = repository;
        _publisher = publisher;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task<IResult> HandleAsync(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new Order
        {
            Id = Guid.NewGuid()
        };

        var adr = request.Dto.Address;
        order.Address = new Address(adr.Country, adr.City, adr.Street, adr.Building, adr.Apartment);
        order.Created = DateTimeOffset.UtcNow;
        order.Customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = request.Dto.Customer.Name,
            Surname = request.Dto.Customer.Surname
        };
        order.Status = OrderStatus.Pending;
        order.Items = request.Dto.OrderItems.Select(item => new OrderItem
        {
            Id = Guid.NewGuid(),
            ItemId = item.ItemId,
            Quantity = item.Quantity
        })
        .ToList();

        _logger.LogInformation(
            "Creating order for customer: {CustomerId}, Items: {ItemsCount}",
            order.Customer.Id,
            order.Items.Count);

        await _publisher.Publish(new OrderCreated
        {
            OrderId = order.Id,
            CustomerId = order.Customer.Id,
            Items = order.Items.Select(item => new OrderItemMessage
            {
                OrderItemId = item.Id,
                ItemId = item.ItemId,
                Quantity = item.Quantity
            }).ToList()
        });

        await _repository.AddAsync(order, cancellationToken);
        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Order created successfully. OrderId: {OrderId}",
            order.Id);

        return Result.Success();
    }
}