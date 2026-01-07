using OrderService.Application.Orders.Queries.GetOrders.DTOs;
using OrderService.Domain.Types;
using Shared.Application.Infrastructure.Cqs;
using Shared.Application.Infrastructure.Results;
using Shared.Application.Infrastructure.Results.Abstractions;
using Shared.Domain.Storage.Abstractions;

namespace OrderService.Application.Orders.Queries.GetOrders;

public class GetOrdersQuery : Query<List<OrderItemDto>>;

public class GetOrdersQueryHandler : QueryHandler<GetOrdersQuery, List<OrderItemDto>>
{
    private readonly IReadOnlyRepository<Order> _repository;
    private readonly ILogger<GetOrdersQueryHandler> _logger;

    public GetOrdersQueryHandler(
        IReadOnlyRepository<Order> repository,
        ILogger<GetOrdersQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task<IResult<List<OrderItemDto>>> HandleAsync(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        var orders = await _repository.ListAsync(cancellationToken: cancellationToken);

        stopwatch.Stop();

        _logger.LogInformation(
            "Retrieved {OrderCount} orders in {ElapsedMilliseconds}ms",
            orders.Count,
            stopwatch.ElapsedMilliseconds);

        return Result<List<OrderItemDto>>.Success(orders.Select(o => new OrderItemDto
        {
            Id = o.Id,
            Status = o.Status
        }).ToList());
    }
}