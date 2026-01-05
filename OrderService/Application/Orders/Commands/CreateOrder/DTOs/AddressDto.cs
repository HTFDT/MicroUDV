namespace OrderService.Application.Orders.Commands.CreateOrder.DTOs;

public class  AddressDto
{
    public string Country { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Street { get; set; } = null!;
    public string Building { get; set; } = null!;
    public string Apartment { get; set; } = null!;
}