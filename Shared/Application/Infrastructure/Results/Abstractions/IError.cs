namespace Shared.Application.Infrastructure.Results.Abstractions;

public interface IError
{
    string Type { get; }
    Dictionary<string, object> Data { get; }
}