using System.Reflection;
using Shared.Application.Infrastructure.Results.Abstractions;

namespace Shared.Application.Infrastructure.Results.Helpers;

public static class ResultFactory
{
    public static TResult Success<TResult>(object? value = default)
        where TResult : IResult
    {
        var type = typeof(TResult);
        object? result = null;
        if (type == typeof(IResult))
        {
            result = typeof(Result).InvokeMember(nameof(Result.Success), BindingFlags.InvokeMethod, null, null, null);
        }
        else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IResult<>))
        {
            var valueType = value?.GetType();
            var genericParamType = type.GetGenericArguments().FirstOrDefault();
            if (value != null && valueType != genericParamType)
                throw new ArgumentException("Invalid value type", nameof(value));
            var pms = type.GetGenericArguments();
            var implType = typeof(Result<>).MakeGenericType(pms);
            result = implType.InvokeMember(nameof(Result<object>.Success), BindingFlags.InvokeMethod, null, null, [value]);
        }
        if (result == null)
            throw new NotSupportedException("Result type is not supported");
        return (TResult)result;
    }
}