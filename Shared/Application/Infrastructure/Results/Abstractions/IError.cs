namespace Shared.Application.Infrastructure.Results.Abstractions;

public interface IError
{
    ErrorType Type { get; }
    Dictionary<string, object> Data { get; }
}