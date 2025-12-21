using OrderService.Domain.Types;
using Shared.Domain.Storage.Abstractions;

namespace OrderService.Domain.Storage.Abstractions;

public interface IOrderRepository : IRepository<Order>;