using Festpay.Onboarding.Application.Interfaces.IRepositories;
using Festpay.Onboarding.Domain.Entities;
using Festpay.Onboarding.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Festpay.Onboarding.Infra.Repositories
{
    public class AccountRepository : BasicRepository, IAccountRepository
    {
        private readonly FestpayContext _context;

        public AccountRepository(FestpayContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Guid> CreateAccount(Account account, CancellationToken cancellationToken)
        {
            await _context.Accounts.AddAsync(account, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return account.Id;
        }

        public Task<Account?> GetAccount(Guid id, CancellationToken cancellationToken)
        {
            return _context.Accounts.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public Task<List<Account>> GetAccounts(CancellationToken cancellationToken)
        {
            return _context.Accounts.AsNoTracking().ToListAsync(cancellationToken);
        }

        public Task<bool> VerifyAccountExistence(string document, CancellationToken cancellationToken)
        {
            return _context.Accounts.AnyAsync(x => x.Document == document, cancellationToken);
        }
    }
}
