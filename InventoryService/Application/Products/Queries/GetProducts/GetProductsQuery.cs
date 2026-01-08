using InventoryService.Application.Products.Queries.GetProducts.DTOs;
using InventoryService.Domain.Storage;
using Shared.Application.Infrastructure.Cqs;
using Shared.Application.Infrastructure.Results;
using Shared.Application.Infrastructure.Results.Abstractions;

namespace InventoryService.Application.Products.Queries.GetProducts;

public class GetProductsQuery : Query<List<ProductItemDto>>;

public class GetProductsQueryHandler : QueryHandler<GetProductsQuery, List<ProductItemDto>>
{
    private readonly IProductRepository _repository;
    private readonly ILogger<GetProductsQueryHandler> _logger;

    public GetProductsQueryHandler(
        IProductRepository repository,
        ILogger<GetProductsQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task<IResult<List<ProductItemDto>>> HandleAsync(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        var products = await _repository.QueryAsync(q => q.Select(p => new ProductItemDto
        {
            Id = p.Id,
            Name = p.Name,
            InStockQuantity = p.InStockQuantity,
            Reservations = p.Reservations.Select(r => new ProductReservationDto
            {
                OrderId = r.OrderId,
                OrderItemId = r.OrderItemId,
                ReservedQuantity = r.Quantity
            }).ToList()
        }), cancellationToken);

        stopwatch.Stop();

        _logger.LogInformation(
            "Retrieved {ProductCount} products in {ElapsedMilliseconds}ms",
            products.Count(),
            stopwatch.ElapsedMilliseconds);

        return Result<List<ProductItemDto>>.Success(products.ToList());
    }
}