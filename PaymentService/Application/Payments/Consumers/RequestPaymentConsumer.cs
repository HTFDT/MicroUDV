using PaymentService.Application.Payments.Commands.RequestPayment;
using Shared.Application.Infrastructure.Cqs.Abstractions;
using Shared.Application.Orders.Messages.Commands;
using Shared.Application.Orders.Messages.Events;
using Shared.MT.Application.Infrastructure.Messenging;
using IResult = Shared.Application.Infrastructure.Results.Abstractions.IResult;

namespace PaymentService.Application.Payments.Consumers;

public class RequestPaymentConsumer(ISender sender) : ConsumerBase<RequestPayment>
{
    public override async Task Consume()
    {
        var result = await sender.SendAsync<RequestPaymentCommand, IResult>(new RequestPaymentCommand(ConsumeContext.Message));
        if (!result.IsSuccessful)
        {
            await ConsumeContext.RespondAsync(new PaymentFailed
            {
                OrderId = ConsumeContext.Message.OrderId,
                Reason = "Оплата не прошла."
            });
            return;
        }
            
        await ConsumeContext.RespondAsync(new PaymentSucceeded
        {
            OrderId = ConsumeContext.Message.OrderId
        });
    }
}