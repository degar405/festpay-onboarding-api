using Festpay.Onboarding.Infra.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

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

public sealed class GetAccountsQueryHandler(FestpayContext dbContext) : IRequestHandler<GetAccountsQuery, ICollection<GetAccountsQueryResponse>>
{
    public async Task<ICollection<GetAccountsQueryResponse>> Handle(
        GetAccountsQuery request,
        CancellationToken cancellationToken
    )
    {
        var accounts = await dbContext.Accounts.ToListAsync(cancellationToken);

        return accounts
            .Select(a => new GetAccountsQueryResponse(
                a.Id,
                a.Name,
                a.Document,
                a.Email,
                a.Phone,
                a.CreatedUtc,
                a.DeactivatedUtc,
                a.Balance
            ))
            .ToList();
    }
}
