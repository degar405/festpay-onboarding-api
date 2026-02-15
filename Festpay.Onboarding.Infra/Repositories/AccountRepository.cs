using Festpay.Onboarding.Application.Common.Constants;
using Festpay.Onboarding.Application.Common.Results;
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

        public async Task<Result<Guid>> CreateAccount(Account account, CancellationToken cancellationToken)
        {
            try 
            { 
                await _context.Accounts.AddAsync(account, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex))
            {
                return Result<Guid>.Conflict(string.Format(ErrorMessageConstants.EntityAlreadyExists, nameof(Account)));
            }

            return Result<Guid>.Ok(account.Id);
        }

        public Task<Account?> GetAccountWithTrack(Guid id, CancellationToken cancellationToken)
        {
            return _context.Accounts.FindAsync(id, cancellationToken).AsTask();
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
