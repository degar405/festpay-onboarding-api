using Festpay.Onboarding.Application.Interfaces.IRepositories;
using Festpay.Onboarding.Infra.Context;

namespace Festpay.Onboarding.Infra.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FestpayContext _context;
        
        public IAccountRepository Accounts { get; }
        public ITransactionRepository Transactions { get; }

        public UnitOfWork(FestpayContext context,
                          IAccountRepository accounts,
                          ITransactionRepository transactions)
        {
            _context = context;
            Accounts = accounts;
            Transactions = transactions;
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken) 
                    => _context.SaveChangesAsync(cancellationToken);
    }
}
