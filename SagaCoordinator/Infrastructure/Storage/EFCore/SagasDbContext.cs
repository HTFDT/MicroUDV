using Microsoft.EntityFrameworkCore;
using Shared.MT.Infrastructure;

namespace SagaCoordinator.Infrastructure.Storage.EFCore;

public class SagasDbContext(DbContextOptions options) : MassTransitDbContextBase(options);