using Shared.Application.Infrastructure.Results.Abstractions;

namespace Shared.Application.Infrastructure.Results;

public abstract class ErrorBase : IError
{
    public abstract ErrorType Type { get; }
    public Dictionary<string, object> Data { get; } = new();
}