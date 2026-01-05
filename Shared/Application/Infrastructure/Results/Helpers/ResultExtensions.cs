using Microsoft.AspNetCore.Mvc;
using Shared.Application.Infrastructure.Results.Abstractions;

namespace Shared.Application.Infrastructure.Results.Helpers;

public static class ResultExtensions
{
    public static IActionResult ToActionResult(this IResult result)
    {
        if (result.IsSuccessful)
            return new OkResult();
        var errors = result.GetErrors();
        var err = errors[0];
        if (err.Type == ErrorType.NotFound)
            return new NotFoundResult();
        return new BadRequestResult();
    }

    public static IActionResult ToActionResult<TValue>(this IResult<TValue> result)
    {
        if (result.IsSuccessful)
            return new OkObjectResult(result.Value);
        var errors = result.GetErrors();
        var err = errors[0];
        if (err.Type == ErrorType.NotFound)
            return new NotFoundResult();
        return new BadRequestResult();
    }
}