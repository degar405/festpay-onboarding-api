namespace Festpay.Onboarding.Application.Interfaces.IRepositories
{
    public interface IBasicRepository
    {
        Task ConfirmModelChanges(CancellationToken cancellationToken);
    }
}
