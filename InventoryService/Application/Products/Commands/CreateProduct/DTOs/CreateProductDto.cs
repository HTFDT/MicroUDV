namespace InventoryService.Application.Products.Commands.CreateProduct.DTOs;

public class CreateProductDto
{
    public string Name { get; set; } = null!;
    public int InStockQuantity { get; set; }
    public double PriceRub { get; set; }
}