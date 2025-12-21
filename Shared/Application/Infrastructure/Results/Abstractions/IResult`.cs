namespace Shared.Application.Infrastructure.Results.Abstractions;

public interface IResult<out TValue> : IResult
{
    TValue Value { get; }
}