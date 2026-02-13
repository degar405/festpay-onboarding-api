namespace Festpay.Onboarding.Application.Interfaces.IRepositories
{
    public interface IUnitOfWork
    {
        IAccountRepository Accounts { get; }
        ITransactionRepository Transactions { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
