using FluentValidation;
using MediatR;
using Entities = Festpay.Onboarding.Domain.Entities;
using Festpay.Onboarding.Application.Interfaces.IRepositories;
using Festpay.Onboarding.Application.Common.Exceptions;
using Festpay.Onboarding.Application.Common.Results;

namespace Festpay.Onboarding.Application.Features.V1.Transaction.Commands;

public sealed record CreateTransactionCommand(
    Guid SourceAccountID,
    Guid DestinationAccountID,
    decimal Value
) : IRequest<Result<Guid>>;

public sealed class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionCommandValidator()
    {
        RuleFor(x => x.SourceAccountID)
            .NotEmpty()
            .WithMessage("SourceAccountID is required.");

        RuleFor(x => x.DestinationAccountID)
            .NotEmpty()
            .WithMessage("DestinationAccountID is required.")
            .NotEqual(x => x.SourceAccountID)
            .WithMessage("Source and destination accounts must be different.");

        RuleFor(x => x.Value)
            .GreaterThan(0m)
            .WithMessage("Value must be greater than zero.");
    }
}

public sealed class CreateTransactionCommandHandler(ITransactionRepository repository, IAccountRepository accountRepository)
    : IRequestHandler<CreateTransactionCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        CreateTransactionCommand request,
        CancellationToken cancellationToken
    )
    {
        var sourceAccount = await accountRepository.GetAccountWithTrack(request.SourceAccountID, cancellationToken);
        if (sourceAccount == null)
            throw new EntityDoesntExistException("Source Account");

        var destinationAccount = await accountRepository.GetAccountWithTrack(request.DestinationAccountID, cancellationToken);
        if (destinationAccount == null)
            throw new EntityDoesntExistException("Destination Account");

        if (destinationAccount.DeactivatedUtc.HasValue || sourceAccount.DeactivatedUtc.HasValue)
            throw new InactiveEntityException(nameof(Entities.Account));

        var transaction = Entities.Transaction.Create(
            request.SourceAccountID,
            request.DestinationAccountID,
            request.Value
        );

        sourceAccount.AfectBalance(-1 * transaction.Value);
        destinationAccount.AfectBalance(transaction.Value);

        var result = await repository.CreateTransaction(transaction, cancellationToken);
        return Result<Guid>.Ok(result);
    }
}