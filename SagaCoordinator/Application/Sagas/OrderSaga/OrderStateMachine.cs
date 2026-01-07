using MassTransit;
using Shared.Application.Orders.Messages.Commands;
using Shared.Application.Orders.Messages.Events;
using Shared.Domain.Types;

namespace SagaCoordinator.Application.Sagas.OrderSaga;

public class OrderStateMachine : MassTransitStateMachine<OrderState>
{
    public State ReserveProcessing { get; private set; } = null!;
    public State Reserved { get; private set; } = null!;
    public State PaymentProcessing { get; private set; } = null!;
    public State Paid { get; private set; } = null!;
    public State Completed { get; private set; } = null!;
    public State Failed { get; private set; } = null!;

    public Event<OrderCreated> OrderCreated { get; private set; } = null!;
    public Event<PaymentFailed> PaymentFailed { get; private set; } = null!;
    public Event<PaymentSucceeded> PaymentSucceeded { get; private set; } = null!;
    public Event<StockFailed> StockFailed { get; private set; } = null!;
    public Event<StockReserved> StockReserved { get; private set; } = null!;

    public OrderStateMachine()
    {
        InstanceState(x => x.CurrentState);

        Event(() => OrderCreated, e => e.CorrelateById(x => x.Message.OrderId));
        Event(() => PaymentSucceeded, e => e.CorrelateById(x => x.Message.OrderId));
        Event(() => PaymentFailed, e => e.CorrelateById(x => x.Message.OrderId));
        Event(() => StockReserved, e => e.CorrelateById(x => x.Message.OrderId));
        Event(() => StockFailed, e => e.CorrelateById(x => x.Message.OrderId));

        Initially(
            When(OrderCreated)
                .ThenAsync(async ctx =>
                {
                    ctx.Saga.UserId = ctx.Message.CustomerId;

                    await ctx.Send(new ReserveStock
                    {
                        OrderId = ctx.Message.OrderId,
                        Products = ctx.Message.Items.Select(i => new StockItemMessage
                        {
                            OrderItemId = i.OrderItemId,
                            ProductId = i.ItemId,
                            Quantity = i.Quantity
                        }).ToList()
                    });
                })
                .TransitionTo(ReserveProcessing)
        );

        During(ReserveProcessing,
            When(StockReserved)
                .ThenAsync(async ctx =>
                {
                    await ctx.Send(new SetOrderStatus
                    {
                        OrderId = ctx.Message.OrderId,
                        Status = OrderStatus.Reserved,
                    });

                    await ctx.Send(new RequestPayment
                    {
                        OrderId = ctx.Message.OrderId,
                        UserId = ctx.Saga.UserId,
                        Summary = ctx.Message.Summary
                    });
                })
                .TransitionTo(PaymentProcessing),
            
            When(StockFailed)
                .ThenAsync(async ctx =>
                {
                    await ctx.Publish(new OrderCompleted
                    {
                        OrderId = ctx.Message.OrderId,
                        IsSuccessful = false,
                        Reason = ctx.Message.Reason,
                        UserId = ctx.Saga.UserId
                    });
                })
                .TransitionTo(Failed)
                .Finalize()
        );

        During(PaymentProcessing, 
            When(PaymentSucceeded)
                .ThenAsync(async ctx =>
                {
                    await ctx.Send(new SetOrderStatus
                    {
                        OrderId = ctx.Message.OrderId,
                        Status = OrderStatus.Paid 
                    });

                    await ctx.Publish(new OrderCompleted
                    {
                        OrderId = ctx.Message.OrderId,
                        IsSuccessful = true,
                        UserId = ctx.Saga.UserId
                    });
                })
                .TransitionTo(Completed)
                .Finalize(),

            When(PaymentFailed)
                .ThenAsync(async ctx =>
                {
                    await ctx.Publish(new OrderCompleted
                    {
                        OrderId = ctx.Message.OrderId,
                        IsSuccessful = false,
                        Reason = ctx.Message.Reason,
                        UserId = ctx.Saga.UserId
                    });
                })
                .TransitionTo(Failed)
                .Finalize()
        );  
    }
}