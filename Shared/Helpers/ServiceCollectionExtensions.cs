using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Infrastructure.Cqs;
using Shared.Application.Infrastructure.Cqs.Abstractions;

namespace Shared.Helpers;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCqs(this IServiceCollection services, Assembly assembly)
    {
        services.AddMediatR(o =>
        {
            o.RegisterServicesFromAssembly(assembly);
        });
        services.AddTransient<ISender, Sender>();
        return services;
    }
}