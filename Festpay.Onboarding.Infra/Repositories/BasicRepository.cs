using Festpay.Onboarding.Application.Interfaces.IRepositories;
using Festpay.Onboarding.Infra.Context;

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
    }
}
