using System.Reflection;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.MT.Helpers;

public static class ServiceCollectionExtensions
{
    private static void UsingRabbitMqInternal(this IBusRegistrationConfigurator config, Action<TransportOptions> transportCfg)
    {
        var opt = new TransportOptions();
        transportCfg.Invoke(opt);

        config.UsingRabbitMq((context, cfg) =>
        {
            cfg.ConfigureEndpoints(context);
            cfg.Host(opt.RabbitMq!.Host, opt.RabbitMq!.VirtualHost, h =>
            {
                h.Username(opt.RabbitMq!.UserName);
                h.Password(opt.RabbitMq!.Password);
            });
        });
    }

    private static void DefaultAddConfigureEndpointsCallbackInternal(this IBusRegistrationConfigurator config)
    {
        config.AddConfigureEndpointsCallback((context,name,cfg) =>
        {
            cfg.UseDelayedRedelivery(r => r.Intervals(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(15), TimeSpan.FromMinutes(30)));
            cfg.UseMessageRetry(r => r.Immediate(5));
            cfg.UseInMemoryOutbox(context);
            cfg.UseInMemoryInboxOutbox(context);
        });
    }

    public static IServiceCollection AddMassTransitTransport(this IServiceCollection services, Action<TransportOptions> cfg)
    {
        var opt = new TransportOptions();
        cfg.Invoke(opt);

        services.AddMassTransit(x =>
        {
            var asm = Assembly.GetExecutingAssembly();
            x.AddConsumers(asm);

            x.SetKebabCaseEndpointNameFormatter();

            x.UsingRabbitMqInternal(cfg);

            x.DefaultAddConfigureEndpointsCallbackInternal();
        });
        return services;
    }

    public static IServiceCollection AddMassTransitTransport<TDbContext>(this IServiceCollection services, Action<TransportOptions> transportCfg, Action<MassTransitOptions> massTransitCfg)
        where TDbContext : DbContext
    {
        var transportOpts = new TransportOptions();
        transportCfg.Invoke(transportOpts);
        var mtOpts = new MassTransitOptions();
        massTransitCfg.Invoke(mtOpts);

        services.AddMassTransit(x =>
        {
            var asm = Assembly.GetExecutingAssembly();
            x.AddConsumers(asm);
            x.AddSagas(asm);

            if (mtOpts.StateMachinesConfig.IsInMemoryPersistance)
            {
                x.SetInMemorySagaRepositoryProvider();
            }
            else
            {
                x.SetEntityFrameworkSagaRepositoryProvider(cfg =>
                {
                    cfg.ExistingDbContext<TDbContext>();
                });
            }

            x.SetKebabCaseEndpointNameFormatter();

            x.UsingRabbitMqInternal(transportCfg);

            x.DefaultAddConfigureEndpointsCallbackInternal();
        });

        return services;
    }
}