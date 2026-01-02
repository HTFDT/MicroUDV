using InventoryService.Domain.Types;
using Shared.Domain.Storage.Abstractions;

namespace InventoryService.Domain.Storage;

public interface IProductRepository : IRepository<Product>;