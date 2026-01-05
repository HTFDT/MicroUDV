using OrderService.Application.Orders.Queries.GetOrders.DTOs;
using OrderService.Domain.Types;
using Shared.Application.Infrastructure.Cqs;
using Shared.Application.Infrastructure.Results;
using Shared.Application.Infrastructure.Results.Abstractions;
using Shared.Domain.Storage.Abstractions;

namespace OrderService.Application.Orders.Queries.GetOrders;

public class GetOrdersQuery : Query<List<OrderItemDto>>;

public class GetOrdersQueryHandler(IReadOnlyRepository<Order> repository) : QueryHandler<GetOrdersQuery, List<OrderItemDto>>
{
    protected override async Task<IResult<List<OrderItemDto>>> HandleAsync(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await repository.ListAsync(cancellationToken: cancellationToken);

        return Result<List<OrderItemDto>>.Success(orders.Select(o => new OrderItemDto
        {
            Id = o.Id,
            Status = o.Status
        }).ToList());
    }
}