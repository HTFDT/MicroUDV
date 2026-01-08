using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Domain.Storage.Abstractions;

namespace Shared.EF.Helpers;

public static class ServiceCollectionExtensions
{
    private static void AddRepositoryInternal(this IServiceCollection services,
        Type repository,
        Type readOnlyRepository,
        Type repositoryImpl)
    {
        services.AddTransient(repository, repositoryImpl);
        services.AddTransient(repositoryImpl);
        services.AddTransient(readOnlyRepository, sp =>
        {
            var repoImpl = sp.GetRequiredService(repository);
            var prop = repoImpl.GetType().GetProperty(nameof(IReadOnlyRepository.IsReadOnly));
            prop!.SetValue(repoImpl, true);
            return repoImpl;
        });
    }
    
    public static IServiceCollection AddRepository<TRepository, TRepositoryImpl>(this IServiceCollection services) 
        where TRepository : class 
        where TRepositoryImpl : TRepository
    {
        var repoType = typeof(TRepository);
        var readOnlyRepoType = repoType.GetInterface(typeof(IReadOnlyRepository<>).Name);
        var implType = typeof(TRepositoryImpl);
        services.AddRepositoryInternal(repoType, readOnlyRepoType!, implType);
        return services;
    }

    public static IServiceCollection AddUnitOfWorkDbcontext<TDbContext>(this IServiceCollection services, Action<DbOptions> cfg) 
        where TDbContext : DbContext, IUnitOfWork
    {
        var opt = new DbOptions();
        cfg.Invoke(opt);
        services.AddDbContext<TDbContext>((sp, o) =>
        {
            o.UseNpgsql(opt.ConnectionString);
        });
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<TDbContext>());
        return services;
    }
}