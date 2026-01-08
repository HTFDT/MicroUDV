using InventoryService.Domain.Storage;
using InventoryService.Domain.Types;
using Shared.Application.Infrastructure.Cqs;
using Shared.Application.Infrastructure.Results;
using Shared.Application.Orders.Messages.Events;
using Shared.Application.Shared.Results.Errors;
using IResult = Shared.Application.Infrastructure.Results.Abstractions.IResult;

namespace InventoryService.Application.Products.Commands;

public class CompleteOrderCommand(OrderCompleted message) : Command
{
    public OrderCompleted Message { get; set; } = message;
}

public class CompleteOrderCommandHandler : CommandHandler<CompleteOrderCommand>
{
    private readonly IProductRepository _repository;
    private readonly ILogger<CompleteOrderCommandHandler> _logger;

    public CompleteOrderCommandHandler(
        IProductRepository repository,
        ILogger<CompleteOrderCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task<IResult> HandleAsync(CompleteOrderCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Completing order. OrderId: {OrderId}, IsSuccessful: {IsSuccessful}",
            request.Message.OrderId,
            request.Message.IsSuccessful);

        var products = await _repository.ListAsync(p => p.Reservations.Any(r => r.OrderId == request.Message.OrderId), cancellationToken);

        if (products.Count == 0)
        {
            _logger.LogWarning(
                "No products found for order. OrderId: {OrderId}",
                request.Message.OrderId);
            return Result.Error(new NotFoundError<Product>(request.Message.OrderId));
        }

        _logger.LogInformation(
            "Found {ProductCount} products with reservations for order: {OrderId}",
            products.Count,
            request.Message.OrderId);

        if (request.Message.IsSuccessful)
        {
            foreach (var product in products)
            {
                product.Reservations = product.Reservations.Where(r => r.OrderId != request.Message.OrderId).ToList();
            }
            await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Order completed successfully. Removed reservations for {ProductCount} products",
                products.Count);

            return Result.Success();
        }

        foreach (var product in products)
        {
            var reservationsToDelete = product.Reservations.Where(r => r.OrderId == request.Message.OrderId).ToList();
            var sum = reservationsToDelete.Sum(r => r.Quantity);
            product.Reservations = product.Reservations.Except(reservationsToDelete).ToList();
            product.InStockQuantity += sum;
        }
        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Order canceled. Returned stock for {ProductCount} products",
            products.Count);

        return Result.Success();
    }
}