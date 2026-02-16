using Festpay.Onboarding.Application.Common.Results;
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

public sealed record GetTransactionsQuery : IRequest<Result<ICollection<GetTransactionsQueryResponse>>>;

public sealed class GetTransactionsQueryHandler(ITransactionRepository repository) : IRequestHandler<GetTransactionsQuery, Result<ICollection<GetTransactionsQueryResponse>>>
{
    public async Task<Result<ICollection<GetTransactionsQueryResponse>>> Handle(
        GetTransactionsQuery request,
        CancellationToken cancellationToken
    )
    {
        var transactions = await repository.GetTransactions(cancellationToken);

        return Result<ICollection<GetTransactionsQueryResponse>>.Ok([.. transactions
            .Select(a => new GetTransactionsQueryResponse(
                a.Id,
                a.SourceAccountID,
                a.DestinationAccountID,
                a.Value,
                a.CreatedUtc,
                a.DeactivatedUtc
            ))]);
    }
}
