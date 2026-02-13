using Festpay.Onboarding.Application.Interfaces.IRepositories;
using MediatR;

namespace Festpay.Onboarding.Application.Features.V1.Account.Queries;

public sealed record GetAccountsQueryResponse(
    Guid Id,
    string Name,
    string Document,
    string Email,
    string Phone,
    DateTime CreatedAt,
    DateTime? DeactivatedAt,
    decimal Balance
);

public sealed record GetAccountsQuery : IRequest<ICollection<GetAccountsQueryResponse>>;

public sealed class GetAccountsQueryHandler(IAccountRepository repository) : IRequestHandler<GetAccountsQuery, ICollection<GetAccountsQueryResponse>>
{
    public async Task<ICollection<GetAccountsQueryResponse>> Handle(
        GetAccountsQuery request,
        CancellationToken cancellationToken
    )
    {
        var accounts = await repository.GetAccounts(cancellationToken);

        return [.. accounts
            .Select(a => new GetAccountsQueryResponse(
                a.Id,
                a.Name,
                a.Document,
                a.Email,
                a.Phone,
                a.CreatedUtc,
                a.DeactivatedUtc,
                a.Balance
            ))];
    }
}
