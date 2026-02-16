using Festpay.Onboarding.Application.Common.Constants;
using Festpay.Onboarding.Application.Common.Results;
using Festpay.Onboarding.Application.Interfaces.IRepositories;
using FluentValidation;
using MediatR;
using Entities = Festpay.Onboarding.Domain.Entities;

namespace Festpay.Onboarding.Application.Features.V1.Transaction.Commands;

public sealed record CancelTransactionCommand(Guid Id) : IRequest<Result>;

public sealed class CancelTransactionCommandValidator : AbstractValidator<CancelTransactionCommand>
{
    public CancelTransactionCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
    }
}

public sealed class CancelTransactionCommandHandler(ITransactionRepository transactionRepository, IAccountRepository accountRepository)
    : IRequestHandler<CancelTransactionCommand, Result>
{
    public async Task<Result> Handle(
        CancelTransactionCommand request,
        CancellationToken cancellationToken
    )
    {
        var transaction = await transactionRepository.GetTransactionWithTracking(request.Id, cancellationToken);
        if (transaction == null)
            return Result<Guid>.NotFound(string.Format(ErrorMessageConstants.EntityDoesntExist, nameof(Entities.Transaction)));
        if (transaction.DeactivatedUtc.HasValue)
            return Result<Guid>.Conflict(string.Format(ErrorMessageConstants.OperationAlreadyPerformed, nameof(Entities.Transaction)));

        var sourceAccount = await accountRepository.GetAccountWithTrack(transaction.SourceAccountID, cancellationToken);
        if (sourceAccount == null)
            return Result<Guid>.NotFound(string.Format(ErrorMessageConstants.EntityDoesntExist, "Source Account"));

        var destinationAccount = await accountRepository.GetAccountWithTrack(transaction.DestinationAccountID, cancellationToken);
        if (destinationAccount == null)
            return Result<Guid>.NotFound(string.Format(ErrorMessageConstants.EntityDoesntExist, "Destination Account"));

        transaction.EnableDisable();
        sourceAccount.AfectBalance(transaction.Value);
        destinationAccount.AfectBalance(-1 * transaction.Value);

        return await transactionRepository.ConfirmModelChanges(cancellationToken, nameof(Entities.Account));
    }
}