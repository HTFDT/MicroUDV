using Shared.Domain.Types;

namespace OrderService.Application.Orders.Queries.GetOrders.DTOs;

public class OrderItemDto
{
    public Guid Id { get; set; }
    public OrderStatus Status { get; set; }
}