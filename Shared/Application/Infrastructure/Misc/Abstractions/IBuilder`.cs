namespace Shared.Application.Infrastructure.Misc.Abstractions;

public interface IBuilder<out T>
{
    T Build();
}