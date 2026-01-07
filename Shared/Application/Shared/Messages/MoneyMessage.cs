using Shared.Domain.Types;

namespace Shared.Application.Shared.Messages;

public class MoneyMessage(Money money)
{
    public double Rub { get ; set; } = money.Rub; 
}