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
public class OrderController : ControllerBase
{
    private readonly ISender _sender;
    private readonly ILogger<OrderController> _logger;

    public OrderController(ISender sender, ILogger<OrderController> logger)
    {
        _sender = sender;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpPost("orders")]
    public async Task<IActionResult> CreateOrder(CreateOrderDto dto)
    {
        _logger.LogInformation(
            "CreateOrder called. Customer: {CustomerName}, Items count: {ItemsCount}, Total quantity: {TotalAmount}",
            dto.Customer?.Name ?? "unknown",
            dto.OrderItems?.Count ?? 0,
            dto.OrderItems?.Sum(i => i.Quantity) ?? 0);

        var result = await _sender.SendAsync<CreateOrderCommand, Abs.IResult>(new CreateOrderCommand(dto));

        if (result.IsSuccessful)
        {
            _logger.LogInformation(
                "Order created successfully. Customer: {CustomerName}",
                dto.Customer?.Name ?? "unknown");
        }
        else
        {
            foreach (var item in result.GetErrors())
            {
                _logger.LogWarning(
                    "Order creation failed. Error: {ErrorType}",
                    item.Type.ToString() ?? "unknown");
            }
        }

        return result.ToActionResult();
    }

    [HttpGet("orders")]
    public async Task<IActionResult> GetOrders()
    {
        _logger.LogInformation("GetOrders called");

        var result = await _sender.SendAsync<GetOrdersQuery, Abs.IResult<List<Queries_OrderItemDto>>>(new GetOrdersQuery());

        if (result.IsSuccessful)
        {
            _logger.LogInformation("Retrieve orders successfully");
        }
        else
        {
            foreach (var item in result.GetErrors())
            {
                _logger.LogWarning(
                    "Order retrieving failed. Error: {ErrorType}",
                    item.Type.ToString() ?? "unknown");
            }
        }

        return result.ToActionResult();
    }
}