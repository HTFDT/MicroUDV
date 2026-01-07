using InventoryService.Domain.Storage;
using Shared.Application.Infrastructure.Cqs;
using Shared.Application.Infrastructure.Results;
using Shared.Application.Orders.Messages.Commands;
using Shared.Application.Shared.Results.Errors;
using InventoryService.Domain.Types;
using Shared.Domain.Types;
using Shared.Application.Infrastructure.Results.Abstractions;

namespace InventoryService.Application.Products.Commands;

public class ReserveStockCommand(ReserveStock message) : Command<Money>
{
    public ReserveStock Message { get; set; } = message;
}

public class ReserveStockCommandHandler(IProductRepository repository) : CommandHandler<ReserveStockCommand, Money>
{
    protected override async Task<IResult<Money>> HandleAsync(ReserveStockCommand request, CancellationToken cancellationToken)
    {
        var productIds = request.Message.Products.Select(p => p.ProductId).ToList();

        var products = await repository.ListAsync(p => productIds.Contains(p.Id), cancellationToken);

        if (products.Count != productIds.Count)
            return Result<Money>.Error(new BadInputDataError(request.Message, "not all products exist"));

        var productIdToData = request.Message.Products.ToDictionary(p => p.ProductId, p => p);
        var productIdToProduct = products.ToDictionary(p => p.Id, p => p);

        var sum = Money.Empty();
        foreach (var productId in productIds)
        {
            var entity = productIdToProduct[productId];
            var data = productIdToData[productId];

            if (entity.Reservations.Any(r => r.OrderItemId == data.OrderItemId))
                continue;
            
            if (data.Quantity > entity.InStockQuantity)
                return Result<Money>.Error(new BadInputDataError(request.Message, $"doesn't have enough product {entity.Id} in stock"));
            
            entity.InStockQuantity -= data.Quantity;

            entity.Reservations.Add(new Reservation
            {
                Quantity = data.Quantity,
                OrderId = request.Message.OrderId,
                OrderItemId = data.OrderItemId,
            });

            sum += entity.Price;
        }

        await repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Money>.Success(sum);
    }
}