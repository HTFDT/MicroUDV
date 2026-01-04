using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Infrastructure.Messenging.Abstractions;
using Shared.MT.Application.Infrastructure.Messenging;

namespace Shared.MT.Helpers;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMassTransitCustom(this IServiceCollection services, Action<MassTransitConfigurationBuilder> cfg)
    {
        var builder = new MassTransitConfigurationBuilder();
        cfg.Invoke(builder);
        var busCfg = builder.Build();

        services.AddMassTransit(x =>
        {
            busCfg.Invoke(x);
        });

        services.AddScoped<DefaultMessenger>();
        services.AddScoped<IMessageSender>(sp => sp.GetRequiredService<DefaultMessenger>());
        services.AddScoped<IMessageSender>(sp => sp.GetRequiredService<DefaultMessenger>());

        return services;
    }
}