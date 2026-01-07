using MassTransit;
using Shared.Application.Orders.Messages.Commands;

namespace Shared.MT.Helpers;

public static class EndpointConventionMapper
{
    public static void MapEndpoints()
    {
        EndpointConvention.Map<RequestPayment>(new Uri("queue:request-payment"));
        EndpointConvention.Map<SetOrderStatus>(new Uri("queue:set-order-status"));
        EndpointConvention.Map<RequestPayment>(new Uri("queue:reserve-stock"));
    }
}