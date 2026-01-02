using InventoryService.Domain.Storage;
using InventoryService.Domain.Types;
using Shared.EF.Infrastructure;

namespace InventoryService.Infrastructure.Storage.EFCore;

public class ProductRepository(InventoryDbContext dbContext) : EFRepository<Product, InventoryDbContext>(dbContext), IProductRepository;