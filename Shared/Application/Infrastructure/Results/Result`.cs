using Shared.Application.Infrastructure.Results.Abstractions;

namespace Shared.Application.Infrastructure.Results;

public class Result<TValue> : Result, IResult<TValue>
{
    private Result(TValue value)
    {
        Value = value;
    }
    
    public TValue Value { get; }

    public new Result<TValue> AddError(IError error)
    {
        return (Result<TValue>)base.AddError(error);
    }

    public static Result<TValue> Success(TValue value)
    {
        return new Result<TValue>(value);
    }

    public new static Result<TValue> Error(IError error)
    {
        var result = new Result<TValue>(default!);
        return result.AddError(error);
    }
}