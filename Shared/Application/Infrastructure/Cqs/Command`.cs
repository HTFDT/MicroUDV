using MediatR;
using Shared.Application.Infrastructure.Cqs.Abstractions;
using Shared.Application.Infrastructure.Results.Abstractions;

namespace Shared.Application.Infrastructure.Cqs;

public abstract class Command<TResult> : IRequest<IResult<TResult>>, ICommand<TResult>;