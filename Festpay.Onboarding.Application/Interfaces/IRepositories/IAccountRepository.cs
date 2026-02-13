using Entities = Festpay.Onboarding.Domain.Entities;

namespace Festpay.Onboarding.Application.Interfaces.IRepositories
{
    public interface IAccountRepository : IBasicRepository
    {
        Task<Guid> CreateAccount(Entities.Account account, CancellationToken cancellationToken);
        Task<List<Entities.Account>> GetAccounts(CancellationToken cancellationToken);
        Task<Entities.Account?> GetAccountWithTrack(Guid id, CancellationToken cancellationToken);
        Task<bool> VerifyAccountExistence(string document, CancellationToken cancellationToken);
    }
}
