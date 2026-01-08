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
public class OrderController(ISender sender) : ControllerBase
{
    [HttpPost("products")]
    public async Task<IActionResult> CreateProduct(CreateProductDto dto)
    {
        var result = await sender.SendAsync<CreateProductCommand, Abs.IResult>(new CreateProductCommand(dto));
        return result.ToActionResult();
    }

    [HttpGet("products")]
    public async Task<IActionResult> GetProducts()
    {
        var result = await sender.SendAsync<GetProductsQuery, Abs.IResult<List<ProductItemDto>>>(new GetProductsQuery());
        return result.ToActionResult();
    }
} 