using Festpay.Onboarding.Application.Interfaces.IRepositories;
using MediatR;

namespace Festpay.Onboarding.Application.Features.V1.Transaction.Queries;

public sealed record GetTransactionsQueryResponse(
    Guid Id,
    Guid SourceAccountID,
    Guid DestinationAccountID,
    decimal Value,
    DateTime CreatedAt,
    DateTime? DeactivatedAt
);

public sealed record GetTransactionsQuery : IRequest<ICollection<GetTransactionsQueryResponse>>;

public sealed class GetTransactionsQueryHandler(ITransactionRepository repository) : IRequestHandler<GetTransactionsQuery, ICollection<GetTransactionsQueryResponse>>
{
    public async Task<ICollection<GetTransactionsQueryResponse>> Handle(
        GetTransactionsQuery request,
        CancellationToken cancellationToken
    )
    {
        var transactions = await repository.GetTransactions(cancellationToken);

        return [.. transactions
            .Select(a => new GetTransactionsQueryResponse(
                a.Id,
                a.SourceAccountID,
                a.DestinationAccountID,
                a.Value,
                a.CreatedUtc,
                a.DeactivatedUtc
            ))];
    }
}
