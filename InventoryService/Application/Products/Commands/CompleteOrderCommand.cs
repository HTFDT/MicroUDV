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

public class CompleteOrderCommandHandler(IProductRepository repository) : CommandHandler<CompleteOrderCommand>
{
    protected override async Task<IResult> HandleAsync(CompleteOrderCommand request, CancellationToken cancellationToken)
    {
        var products = await repository.ListAsync(p => p.Reservations.Any(r => r.OrderId == request.Message.OrderId), cancellationToken);

        if (products.Count == 0)
            return Result.Error(new NotFoundError<Product>(request.Message.OrderId));
        
        if (request.Message.IsSuccessful)
        {
            foreach (var product in products)
            {
                product.Reservations = product.Reservations.Where(r => r.OrderId != request.Message.OrderId).ToList();
            }
            return Result.Success();
        }

        foreach (var product in products)
        {
            var reservationsToDelete = product.Reservations.Where(r => r.OrderId == request.Message.OrderId).ToList();
            var sum = reservationsToDelete.Sum(r => r.Quantity);
            product.Reservations = product.Reservations.Except(reservationsToDelete).ToList();
            product.InStockQuantity += sum;
        }
        return Result.Success();
    }
}