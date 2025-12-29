namespace Shared.Domain.Storage.Types;

public record Money
{
    private double _value;

    public double Rub => _value;

    public static Money FromRub(double value)
    {
        return new Money
        {
            _value = value
        };
    }
}