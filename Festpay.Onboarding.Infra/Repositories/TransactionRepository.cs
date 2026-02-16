using Festpay.Onboarding.Application.Common.Constants;
using Festpay.Onboarding.Application.Common.Results;
using Festpay.Onboarding.Application.Interfaces.IRepositories;
using Festpay.Onboarding.Domain.Entities;
using Festpay.Onboarding.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Festpay.Onboarding.Infra.Repositories
{
    public class TransactionRepository : BasicRepository, ITransactionRepository
    {
        private readonly FestpayContext _context;

        public TransactionRepository(FestpayContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Result<Guid>> CreateTransaction(Transaction transaction, CancellationToken cancellationToken)
        {
            await _context.Transactions.AddAsync(transaction, cancellationToken);
            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Result<Guid>.Conflict(string.Format(ErrorMessageConstants.ConcurrentOperationDetected, nameof(Account)));
            }

            return Result<Guid>.Ok(transaction.Id);
        }

        public Task<Transaction?> GetTransaction(Guid id, CancellationToken cancellationToken)
        {
            return _context.Transactions.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public Task<List<Transaction>> GetTransactions(CancellationToken cancellationToken)
        {
            return _context.Transactions.AsNoTracking().ToListAsync(cancellationToken);
        }

        public Task<Transaction?> GetTransactionWithTracking(Guid id, CancellationToken cancellationToken)
        {
            return _context.Transactions.FindAsync(id, cancellationToken).AsTask();
        }
    }
}
