using Microsoft.EntityFrameworkCore;

namespace Shared.MT.Helpers;

public class MassTransitOptions
{
    public class SagaStateMachinesConfig
    {
        public bool IsInMemoryPersistance { get; set; }
    }

    public SagaStateMachinesConfig StateMachinesConfig { get; set; } = null!;
}