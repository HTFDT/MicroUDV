using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.EF.Helpers;

public static class DatabaseUpdater
{
    public static void UpdateDatabase<TDbContext>(IServiceProvider sp)
        where TDbContext : DbContext
    {
        var dbContext = sp.GetRequiredService<TDbContext>();
        dbContext.Database.Migrate();
    }
}