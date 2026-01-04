namespace Shared.Application.Infrastructure.Messenging.Abstractions;

public interface IMessageSender
{
    Task Send<TMessage>(TMessage message) where TMessage : class;
}