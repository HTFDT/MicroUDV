using OrderService.Domain.Storage.Abstractions;
using OrderService.Domain.Types;
using Shared.EF.Infrastructure;

namespace OrderService.Infrastructure.Storage.EFCore;

public class OrderRepository(OrderDbContext dbContext) : EFRepository<Order, OrderDbContext>(dbContext), IOrderRepository;