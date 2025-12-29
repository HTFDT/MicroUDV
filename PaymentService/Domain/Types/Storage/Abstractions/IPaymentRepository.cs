using PaymentService.Domain.Types;
using Shared.Domain.Storage.Abstractions;

namespace PaymentService.Domain.Storage.Abstractions;

public interface IPaymentRepository : IRepository<Payment>;