using System.Reflection;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Infrastructure.Misc;
using Shared.Application.Infrastructure.Misc.Abstractions;

namespace Shared.MT.Helpers;

public class MassTransitConfigurationBuilder : IBuilder<Action<IBusRegistrationConfigurator>>
{
    private readonly TransportConfigurationBuilder _tcb = new();
    private readonly SagaStateMachinesConfigurationBuilder _smcb = new();
    private FlagAndValue<Assembly> _consumersAdded = new (false);
    private FlagAndValue<Assembly> _stateMachinesAdded = new (false);
    private FlagAndValue<Action<IBusRegistrationConfigurator>> _efOutbox = new (false);
    private FlagAndValue<ConfigureEndpointsCallbackOptions> _cfgEndpointsCallback = new (false);

    public TransportConfigurationBuilder Transport()
    {
        return _tcb;
    }

    public SagaStateMachinesConfigurationBuilder AddSagaStateMachines(Assembly asm)
    {
        _stateMachinesAdded = new FlagAndValue<Assembly>(true, asm);
        return _smcb;
    }

    public void AddConsumers(Assembly assembly)
    {
        _consumersAdded = new FlagAndValue<Assembly>(true, assembly);;
    }

    public void AddEntityFrameworkOutbox<TDbContext>() where TDbContext : DbContext
    {
        var cfg = (IBusRegistrationConfigurator x) =>
        {
            x.AddEntityFrameworkOutbox<TDbContext>(c =>
            {
                c.QueryDelay = TimeSpan.FromSeconds(1);
                c.UsePostgres();
                c.UseBusOutbox();
            });
        };
        _efOutbox = new (true, cfg);
    }

    public void AddConfigureEndpointsCallback(Action<ConfigureEndpointsCallbackOptions> cfg)
    {
        var opts = new ConfigureEndpointsCallbackOptions();
        cfg.Invoke(opts);
        _cfgEndpointsCallback = new (true, opts);
    }

    public Action<IBusRegistrationConfigurator> Build()
    {
        var action = (IBusRegistrationConfigurator x) =>
        {
            x.SetKebabCaseEndpointNameFormatter();
            if (_consumersAdded.Item1)
                x.AddConsumers(_consumersAdded.Item2);
            if (_stateMachinesAdded.Item1)
            {
                x.AddSagaStateMachines(_stateMachinesAdded.Item2);
                _smcb.Build().Invoke(x);
            }
            
            if (_efOutbox.Item1)
                _efOutbox.Item2!.Invoke(x);

            _tcb.Build().Invoke(x);
            
            if (_cfgEndpointsCallback.Item1)
            {
                var opts = _cfgEndpointsCallback.Item2!;
                x.AddConfigureEndpointsCallback((context, name, cfg) =>
                {
                    if (opts.UseRedelivery)
                        cfg.UseDelayedRedelivery(r => r.Intervals(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(15), TimeSpan.FromMinutes(30)));
                    if (opts.Retries > 0)
                        cfg.UseMessageRetry(r => r.Immediate((int)opts.Retries));
                    if (opts.UseInMemoryOutbox)
                        cfg.UseInMemoryOutbox(context);
                });
            }      
        };
        return action;
    }
}

public class TransportConfigurationBuilder : IBuilder<Action<IBusRegistrationConfigurator>>
{
    private bool _usingInMemory;
    private FlagAndValue<Action<RabbitMqOptions>> _rabbitMq = new (false, null);

    public void UsingInMemory()
    {
        _usingInMemory = true;
    }

    public void UsingRabbitMq(Action<RabbitMqOptions> cfg)
    {
        _rabbitMq = new (true, cfg);
    }

    public Action<IBusRegistrationConfigurator> Build()
    {
        return x =>
        {
            if (_rabbitMq.Item1)
            {
                var opts = new RabbitMqOptions();
                _rabbitMq.Item2!.Invoke(opts);
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                    cfg.Host(opts.Host, opts.VirtualHost, h =>
                    {
                        h.Username(opts.UserName);
                        h.Password(opts.Password);
                    });
                });
                return;
            }
            if (_usingInMemory)
            {
                x.UsingInMemory();
                return;
            }
        };
    }
}

public class SagaStateMachinesConfigurationBuilder : IBuilder<Action<IBusRegistrationConfigurator>>
{
    private bool _usingInMemory;
    private FlagAndValue<Action<IBusRegistrationConfigurator>> _usingEf = new(false, null);

    public void UsingInMemoryRepository()
    {
        _usingInMemory = true;
    }

    public void UsingEntityFrameworkRepository<TDbContext>(Action<EntityFrameworkRepositoryOptions> efCfg) where TDbContext : DbContext
    {
        var cfg = (IBusRegistrationConfigurator x) =>
        {
            var opts = new EntityFrameworkRepositoryOptions();
            efCfg.Invoke(opts);
            x.SetEntityFrameworkSagaRepositoryProvider(i =>
            {
                i.ConcurrencyMode = ConcurrencyMode.Pessimistic;
                i.UsePostgres();
                if (opts.UseExistingDbContext)
                    i.ExistingDbContext<TDbContext>();
                else
                    i.AddDbContext<DbContext, TDbContext>((provider, builder) =>
                    {
                        builder.UseNpgsql(opts.ConnectionString, m =>
                        {
                            m.MigrationsAssembly(opts.MigrationsAssembly!);
                            m.MigrationsHistoryTable(opts.MigrationsHistoryTable!);
                        });
                    });
            });
        };
        _usingEf = new (true, cfg);
    }

    public Action<IBusRegistrationConfigurator> Build()
    {
        return x =>
        {
            if (_usingInMemory)
            {
                x.SetInMemorySagaRepositoryProvider();
                return;
            }
            if (_usingEf.Item1)
            {
                _usingEf.Item2!.Invoke(x);
                return;
            }
        };
    }
}

public class RabbitMqOptions
{
    public string Host { get; set; } = null!;
    public string VirtualHost { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class ConfigureEndpointsCallbackOptions
{
    public bool UseRedelivery { get; set; }
    public uint Retries { get; set; }
    public bool UseInMemoryOutbox { get; set; }
}

public class EntityFrameworkRepositoryOptions
{
    public bool UseExistingDbContext { get; set; }
    public string? ConnectionString { get; set; }
    public Assembly? MigrationsAssembly { get; set; }
    public string? MigrationsHistoryTable { get; set; }
}
