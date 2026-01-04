using System.Reflection;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Infrastructure.Messenging.Abstractions;
using Shared.MT.Application.Infrastructure.Messenging;

namespace Shared.MT.Helpers;

public static class ServiceCollectionExtensions
{
    private static void UsingRabbitMqInternal(this IBusRegistrationConfigurator config, TransportOptions.RabbitMqOptions opts)
    {
        config.UsingRabbitMq((context, cfg) =>
        {
            cfg.ConfigureEndpoints(context);
            cfg.Host(opts.Host, opts.VirtualHost, h =>
            {
                h.Username(opts.UserName);
                h.Password(opts.Password);
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

            if (opt.IsInMemoryTransport)
                x.UsingInMemory();
            else
                x.UsingRabbitMqInternal(opt.RabbitMq!);

            x.DefaultAddConfigureEndpointsCallbackInternal();
        });

        services.AddScoped<DefaultMessenger>();
        services.AddScoped<IMessageSender>(sp => sp.GetRequiredService<DefaultMessenger>());
        services.AddScoped<IMessageSender>(sp => sp.GetRequiredService<DefaultMessenger>());

        return services;
    }

    public static IServiceCollection AddMassTransitTransportWithSagas<TDbContext>(this IServiceCollection services, Action<TransportOptions> transportCfg, Action<MassTransitOptions> massTransitCfg)
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

            if (transportOpts.IsInMemoryTransport)
                x.UsingInMemory();
            else
                x.UsingRabbitMqInternal(transportOpts.RabbitMq!);

            x.DefaultAddConfigureEndpointsCallbackInternal();
        });
        
        services.AddScoped<DefaultMessenger>();
        services.AddScoped<IMessageSender>(sp => sp.GetRequiredService<DefaultMessenger>());
        services.AddScoped<IMessageSender>(sp => sp.GetRequiredService<DefaultMessenger>());

        return services;
    }
}