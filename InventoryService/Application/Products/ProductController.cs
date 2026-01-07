using InventoryService.Application.Products.Commands.CreateProduct;
using InventoryService.Application.Products.Commands.CreateProduct.DTOs;
using Microsoft.AspNetCore.Mvc;
using Shared.Application.Infrastructure.Cqs.Abstractions;
using Shared.Application.Infrastructure.Results.Helpers;
using IResult = Shared.Application.Infrastructure.Results.Abstractions.IResult;

namespace InventoryService.Application.Products;

[ApiController]
[Route("api")]
public class OrderController(ISender sender) : ControllerBase
{
    public async Task<IActionResult> CreateProduct(CreateProductDto dto)
    {
        var result = await sender.SendAsync<CreateProductCommand, IResult>(new CreateProductCommand(dto));
        return result.ToActionResult();
    }
} 