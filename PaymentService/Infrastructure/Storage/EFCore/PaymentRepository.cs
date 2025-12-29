using PaymentService.Domain.Storage.Abstractions;
using PaymentService.Domain.Types;
using Shared.EF.Infrastructure;

namespace PaymentService.Infrastructure.Storage.EFCore;

public class PaymentRepository(PaymentDbContext dbContext) : EFRepository<Payment, PaymentDbContext>(dbContext), IPaymentRepository;