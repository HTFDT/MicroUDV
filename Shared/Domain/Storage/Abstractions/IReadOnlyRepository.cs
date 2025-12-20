namespace Shared.Domain.Storage.Abstractions;

public interface IReadOnlyRepository
{
    bool IsReadOnly { get; set; }
}