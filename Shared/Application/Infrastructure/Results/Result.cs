using Shared.Application.Infrastructure.Results.Abstractions;

namespace Shared.Application.Infrastructure.Results;

public class Result : IResult
{
    private readonly List<IError> _errors = [];
    
    public bool IsSuccessful => _errors.Count == 0;
    
    public IReadOnlyList<IError> GetErrors() => _errors.AsReadOnly();
    
    public Result AddError(IError error)
    {
        ArgumentNullException.ThrowIfNull(error);
        _errors.Add(error);
        return this;
    }

    public static Result Success() => new();

    public static Result Error(IError error)
    {
        var result = new Result();
        return result.AddError(error);
    }
}