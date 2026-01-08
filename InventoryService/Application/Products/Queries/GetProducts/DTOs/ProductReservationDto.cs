namespace InventoryService.Application.Products.Queries.GetProducts.DTOs;

public class ProductReservationDto
{
    public Guid OrderId { get; set; }
    public Guid OrderItemId { get; set; }
    public int ReservedQuantity { get; set; }
}