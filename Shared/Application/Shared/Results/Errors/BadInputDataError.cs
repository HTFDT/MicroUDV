using Shared.Application.Infrastructure.Results;
using Shared.Application.Infrastructure.Results.Abstractions;

namespace Shared.Application.Shared.Results.Errors;

public class BadInputDataError : IError
{
    public ErrorType Type => ErrorType.BadInputData;

    public Dictionary<string, object> Data => new();

    public BadInputDataError(object data, string reason)
    {
        Data["data"] = data;
        Data["reason"] = reason;
    }
}