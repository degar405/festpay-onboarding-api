using Entities = Festpay.Onboarding.Domain.Entities;

namespace Festpay.Onboarding.Application.Interfaces.IRepositories;

public interface ITransactionRepository : IBasicRepository
{
    Task<Guid> CreateTransaction(Entities.Transaction transaction, CancellationToken cancellationToken);
    Task<Entities.Transaction?> GetTransactionWithTracking(Guid id, CancellationToken cancellationToken);
    Task<Entities.Transaction?> GetTransaction(Guid id, CancellationToken cancellationToken);
    Task<List<Entities.Transaction>> GetTransactions(CancellationToken cancellationToken);
}