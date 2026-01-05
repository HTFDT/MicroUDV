namespace OrderService.Application.Orders.Commands.CreateOrder.DTOs;

public class OrderItemDto
{
    public Guid ItemId { get; set; }
    public int Quantity { get; set; }
}