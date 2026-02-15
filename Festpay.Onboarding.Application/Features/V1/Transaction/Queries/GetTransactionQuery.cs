using Festpay.Onboarding.Application.Common.Constants;
using Festpay.Onboarding.Application.Common.Results;
using Festpay.Onboarding.Application.Interfaces.IRepositories;
using FluentValidation;
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

public sealed record GetTransactionQuery(Guid Id) : IRequest<Result<GetTransactionQueryResponse>>;

public sealed class GetAccountByIdQueryValidator
    : AbstractValidator<GetTransactionQuery>
{
    public GetAccountByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.");
    }
}

public sealed class GetTransactionQueryHandler(ITransactionRepository repository) : IRequestHandler<GetTransactionQuery, Result<GetTransactionQueryResponse>>
{
    public async Task<Result<GetTransactionQueryResponse>> Handle(
        GetTransactionQuery request,
        CancellationToken cancellationToken
    )
    {
        var transaction = await repository.GetTransaction(request.Id, cancellationToken);
        if (transaction == null)
            return Result<GetTransactionQueryResponse>.NotFound(string.Format(ErrorMessageConstants.EntityDoesntExist, nameof(Entities.Transaction)));

        return Result<GetTransactionQueryResponse>.Ok(new GetTransactionQueryResponse(
                transaction.Id,
                transaction.SourceAccountID,
                transaction.DestinationAccountID,
                transaction.Value,
                transaction.CreatedUtc,
                transaction.DeactivatedUtc
            ));
    }
}
