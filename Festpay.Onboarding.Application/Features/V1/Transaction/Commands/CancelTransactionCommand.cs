using Festpay.Onboarding.Application.Common.Exceptions;
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

public sealed class CancelTransactionCommandHandler(IUnitOfWork uow)
    : IRequestHandler<CancelTransactionCommand, Result>
{
    public async Task<Result> Handle(
        CancelTransactionCommand request,
        CancellationToken cancellationToken
    )
    {
        var transaction = await uow.Transactions.GetTransactionWithTracking(request.Id, cancellationToken);
        if (transaction == null)
            throw new EntityDoesntExistException(nameof(Entities.Transaction));
        if (transaction.DeactivatedUtc.HasValue)
            throw new OperationAlreadyPerformedException(nameof(Entities.Transaction));
        
        var sourceAccount = await uow.Accounts.GetAccountWithTrack(transaction.SourceAccountID, cancellationToken);
        if (sourceAccount == null)
            throw new EntityDoesntExistException("Source Account");

        var destinationAccount = await uow.Accounts.GetAccountWithTrack(transaction.DestinationAccountID, cancellationToken);
        if (destinationAccount == null)
            throw new EntityDoesntExistException("Destination Account");

        transaction.EnableDisable();
        sourceAccount.AfectBalance(transaction.Value);
        destinationAccount.AfectBalance(-1 * transaction.Value);

        await uow.SaveChangesAsync(cancellationToken);
        return Result.Ok();
    }
}