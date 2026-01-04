using MassTransit;
using Shared.Application.Infrastructure.Messenging.Abstractions;

namespace Shared.MT.Application.Infrastructure.Messenging;

public class DefaultMessenger(IPublishEndpoint publishEndpoint, ISendEndpointProvider bus) : IMessageSender, IMessagePublisher
{
    public Task Publish<TMessage>(TMessage message) where TMessage : class
    {
        return publishEndpoint.Publish(message);
    }

    public Task Send<TMessage>(TMessage message) where TMessage : class
    {
        return bus.Send(message);
    }
}