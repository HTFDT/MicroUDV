namespace InventoryService.Application.Products.Queries.GetProducts.DTOs;

public class ProductItemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public int InStockQuantity { get; set; }
    public List<ProductReservationDto> Reservations { get; set; } = [];
}