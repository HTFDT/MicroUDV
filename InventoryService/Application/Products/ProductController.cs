using InventoryService.Application.Products.Commands.CreateProduct;
using InventoryService.Application.Products.Commands.CreateProduct.DTOs;
using InventoryService.Application.Products.Queries.GetProducts;
using InventoryService.Application.Products.Queries.GetProducts.DTOs;
using Microsoft.AspNetCore.Mvc;
using Shared.Application.Infrastructure.Cqs.Abstractions;
using Shared.Application.Infrastructure.Results.Helpers;
using Abs = Shared.Application.Infrastructure.Results.Abstractions;

namespace InventoryService.Application.Products;

[ApiController]
[Route("api")]
public class ProductController : ControllerBase
{
    private readonly ISender _sender;
    private readonly ILogger<ProductController> _logger;

    public ProductController(ISender sender, ILogger<ProductController> logger)
    {
        _sender = sender;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpPost("products")]
    public async Task<IActionResult> CreateProduct(CreateProductDto dto)
    {
        _logger.LogInformation(
            "CreateProduct called. Product: {ProductName}, Price: {Price}, Stock: {Stock}",
            dto.Name ?? "unknown",
            dto.PriceRub,
            dto.InStockQuantity);

        var result = await _sender.SendAsync<CreateProductCommand, Abs.IResult>(new CreateProductCommand(dto));

        if (result.IsSuccessful)
        {
            _logger.LogInformation(
                "Product created successfully. Product: {CustomerName}",
                dto.Name ?? "unknown");
        }
        else
        {
            foreach (var item in result.GetErrors())
            {
                _logger.LogWarning(
                    "Product creation failed. Error: {ErrorType}",
                    item.Type.ToString() ?? "unknown");
            }
        }

        return result.ToActionResult();
    }

    [HttpGet("products")]
    public async Task<IActionResult> GetProducts()
    {
        _logger.LogInformation("GetProducts called");

        var result = await _sender.SendAsync<GetProductsQuery, Abs.IResult<List<ProductItemDto>>>(new GetProductsQuery());

        if (result.IsSuccessful)
        {
            _logger.LogInformation("Retrieve products successfully");
        }
        else
        {
            foreach (var item in result.GetErrors())
            {
                _logger.LogWarning(
                    "Product retrieving failed. Error: {ErrorType}",
                    item.Type.ToString() ?? "unknown");
            }
        }

        return result.ToActionResult();
    }
} 