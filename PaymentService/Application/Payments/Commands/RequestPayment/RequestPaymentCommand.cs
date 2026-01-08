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

public class RequestPaymentCommandHandler : CommandHandler<RequestPaymentCommand>
{
    private readonly PaymentRepository _repository;
    private readonly ILogger<RequestPaymentCommandHandler> _logger;

    public RequestPaymentCommandHandler(
        PaymentRepository repository,
        ILogger<RequestPaymentCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task<IResult> HandleAsync(RequestPaymentCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Creating payment. OrderId: {OrderId}, CustomerId: {CustomerId}, Amount: {Amount} RUB",
            request.Message.OrderId,
            request.Message.UserId,
            request.Message.Summary.Rub);

        var payment = new Payment
        {
            OrderId = request.Message.OrderId,
            CustomerId = request.Message.UserId,
            Summary = Money.FromRub(request.Message.Summary.Rub)
        };

        await _repository.AddAsync(payment, cancellationToken);
        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Payment created successfully. PaymentId: {PaymentId}, OrderId: {OrderId}",
            payment.Id,
            request.Message.OrderId);

        return Result.Success();
    }
}