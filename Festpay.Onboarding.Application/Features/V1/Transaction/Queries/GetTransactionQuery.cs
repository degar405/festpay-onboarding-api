using Festpay.Onboarding.Application.Common.Exceptions;
using Festpay.Onboarding.Application.Interfaces.IRepositories;
using MediatR;
using Entities = Festpay.Onboarding.Domain.Entities;

namespace Festpay.Onboarding.Application.Features.V1.Transaction.Queries;

public sealed record GetTransactionQueryResponse(
    Guid Id,
    Guid SourceAccountID,
    Guid DestinationAccountID,
    decimal Value,
    DateTime CreatedAt,
    DateTime? DeactivatedAt
);

public sealed record GetTransactionQuery(Guid Id) : IRequest<GetTransactionQueryResponse>;

public sealed class GetTransactionQueryHandler(ITransactionRepository repository) : IRequestHandler<GetTransactionQuery, GetTransactionQueryResponse>
{
    public async Task<GetTransactionQueryResponse> Handle(
        GetTransactionQuery request,
        CancellationToken cancellationToken
    )
    {
        var transaction = await repository.GetTransaction(request.Id, cancellationToken);
        if (transaction == null)
            throw new EntityDoesntExistException(nameof(Entities.Transaction));

        return new GetTransactionQueryResponse(
                transaction.Id,
                transaction.SourceAccountID,
                transaction.DestinationAccountID,
                transaction.Value,
                transaction.CreatedUtc,
                transaction.DeactivatedUtc
            );
    }
}
