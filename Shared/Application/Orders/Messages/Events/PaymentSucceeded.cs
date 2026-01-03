namespace Shared.Application.Orders.Messages.Events;

public class PaymentSucceeded
{
    public Guid OrderId { get; set; }
}