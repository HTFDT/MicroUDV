namespace OrderService.Domain.Types;

public enum OrderStatus
{
    Pending,
    Reserved,
    Paid,
    Completed,
    Canceled
}