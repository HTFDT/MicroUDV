using MassTransit;

namespace SagaCoordinator.Application.Sagas.OrderSaga;

public class OrderState : SagaStateMachineInstance
{
    public Guid CorrelationId { get ; set; }
    public Guid UserId { get; set; }
    public string CurrentState { get; set; } = null!;
}