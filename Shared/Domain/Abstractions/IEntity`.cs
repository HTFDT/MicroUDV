namespace Shared.Domain.Abstractions;

public interface IEntity<TKey> : IEntity where TKey : IEquatable<TKey>, IComparable<TKey>
{
    TKey Id { get; init; }
}