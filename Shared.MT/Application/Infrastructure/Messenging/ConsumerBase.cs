using Shared.Application.Infrastructure.Messenging.Abstractions;

namespace Shared.MT.Application.Infrastructure.Messenging;

public abstract class ConsumerBase<TMessage> : MassTransit.IConsumer<TMessage>, IConsumer<TMessage>
    where TMessage : class
{
    protected MassTransit.ConsumeContext<TMessage> ConsumeContext = null!;

    public abstract Task Consume();

    public Task Consume(MassTransit.ConsumeContext<TMessage> context)
    {
        ConsumeContext = context;
        return Consume();
    }
}