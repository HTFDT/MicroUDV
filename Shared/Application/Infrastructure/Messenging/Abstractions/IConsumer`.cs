namespace Shared.Application.Infrastructure.Messenging.Abstractions;

public interface IConsumer<in TMessage> where TMessage : class
{
    Task Consume();
}