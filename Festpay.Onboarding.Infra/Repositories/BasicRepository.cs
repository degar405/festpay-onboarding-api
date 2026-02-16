using Festpay.Onboarding.Application.Common.Constants;
using Festpay.Onboarding.Application.Interfaces.IRepositories;
using Festpay.Onboarding.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Festpay.Onboarding.Infra.Repositories
{
    public abstract class BasicRepository : IBasicRepository
    {
        private readonly FestpayContext _context;

        public BasicRepository(FestpayContext context)
        {
            _context = context;
        }

        public async Task<Result> ConfirmModelChanges(CancellationToken cancellationToken, string entity)
        {
            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Result.Conflict(string.Format(ErrorMessageConstants.ConcurrentOperationDetected, entity));
            }

            return Result.Ok();
        }

        protected static bool IsUniqueConstraintViolation(DbUpdateException ex)
        {
            return ex.InnerException?.Message.Contains("UNIQUE") == true;
        }
    }
}
