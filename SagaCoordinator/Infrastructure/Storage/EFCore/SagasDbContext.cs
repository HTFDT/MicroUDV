using Microsoft.EntityFrameworkCore;
using Shared.EF.Infrastructure;

namespace SagaCoordinator.Infrastructure.Storage.EFCore;

public class SagasDbContext(DbContextOptions options) : DbContextBase(options);