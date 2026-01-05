using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Orders.Commands.CreateOrder;
using OrderService.Application.Orders.Commands.CreateOrder.DTOs;
using OrderService.Application.Orders.Queries.GetOrders;
using Shared.Application.Infrastructure.Cqs.Abstractions;
using Shared.Application.Infrastructure.Results.Helpers;
using Queries_OrderItemDto = OrderService.Application.Orders.Queries.GetOrders.DTOs.OrderItemDto;
using Abs = Shared.Application.Infrastructure.Results.Abstractions;

namespace OrderService.Application.Orders;

[ApiController]
[Route("api")]
public class OrderController(ISender sender) : ControllerBase
{
    [HttpPost("orders")]
    public async Task<IActionResult> CreateOrder(CreateOrderDto dto)
    {
        var result = await sender.SendAsync<CreateOrderCommand, Abs.IResult>(new CreateOrderCommand(dto));
        return result.ToActionResult();
    }

    [HttpGet("orders")]
    public async Task<IActionResult> GetOrders()
    {
        var result = await sender.SendAsync<GetOrdersQuery, Abs.IResult<List<Queries_OrderItemDto>>>(new GetOrdersQuery());
        return result.ToActionResult();
    }
}