using Shared.Application.Infrastructure.Cqs;
using RequestPaymentMessage = Shared.Application.Orders.Messages.Commands.RequestPayment;
using IResult = Shared.Application.Infrastructure.Results.Abstractions.IResult;
using PaymentService.Infrastructure.Storage.EFCore;
using PaymentService.Domain.Types;
using Shared.Domain.Types;
using Shared.Application.Infrastructure.Results;

namespace PaymentService.Application.Payments.Commands.RequestPayment;

public class RequestPaymentCommand(RequestPaymentMessage message) : Command
{
    public RequestPaymentMessage Message { get; set; } = message;
}

public class RequestPaymentCommandHandler(PaymentRepository repository) : CommandHandler<RequestPaymentCommand>
{
    protected override async Task<IResult> HandleAsync(RequestPaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = new Payment
        {
            OrderId = request.Message.OrderId,
            CustomerId = request.Message.UserId,
            Summary = Money.FromRub(request.Message.Summary.Rub)
        };

        await repository.AddAsync(payment, cancellationToken);
        await repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}