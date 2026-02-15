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

        public async Task ConfirmModelChanges(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        protected static bool IsUniqueConstraintViolation(DbUpdateException ex)
        {
            return ex.InnerException?.Message.Contains("UNIQUE") == true;
        }
    }
}
