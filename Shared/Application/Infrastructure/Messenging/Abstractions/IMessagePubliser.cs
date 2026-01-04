namespace Shared.Application.Infrastructure.Messenging.Abstractions;

public interface IMessagePublisher
{
    Task Publish<TMessage>(TMessage message) where TMessage : class;
}