namespace OrderService.Application.Orders.Commands.CreateOrder.DTOs;

public class CreateOrderDto
{
    public AddressDto Address { get; set; } = null!;
    public CustomerDto Customer { get; set; } = null!;
    public List<OrderItemDto> OrderItems { get; set; } = [];
}