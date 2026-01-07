using Shared.Application.Infrastructure.Results;
using Shared.Application.Infrastructure.Results.Abstractions;
using Shared.Domain.Abstractions;

namespace Shared.Application.Shared.Results.Errors;

public class NotFoundError<TEntity> : IError where TEntity : IEntity
{
    public ErrorType Type => ErrorType.NotFound;

    public Dictionary<string, object> Data => new();

    public NotFoundError(params object[] keys) 
    {
        Data["keys"] = keys;
        Data["entityType"] = typeof(TEntity);
    }
}