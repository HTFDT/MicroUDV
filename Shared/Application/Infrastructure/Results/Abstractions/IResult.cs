namespace Shared.Application.Infrastructure.Results.Abstractions;

public interface IResult
{
    bool IsSuccessful { get; }
    IReadOnlyList<IError> GetErrors();
}