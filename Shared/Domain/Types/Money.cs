namespace Shared.Domain.Types;

public record Money
{
    private double _value;
    private double Value
    {
        get => _value;
        set
        {
            if (_value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), "value of Money shouldn't be less than zero");
            _value = value;
        }
    }

    private Money(double value)
    {
        Value = value;
    }

    public static Money Add(Money left, Money right)
    {
        return new Money(left.Value + right.Value);
    }

    public static Money operator +(Money left, Money right)
    {
        return Add(left, right);
    }

    public static Money Subtract(Money left, Money right)
    {
        return new Money(left.Value - right.Value);
    }

    public static Money operator -(Money left, Money right)
    {
        return Subtract(left, right);
    }

    public double Rub => Value;

    public static Money Empty()
    {
        return new Money(0);
    }

    public static Money FromRub(double value)
    {
        return new Money(value);
    }
}