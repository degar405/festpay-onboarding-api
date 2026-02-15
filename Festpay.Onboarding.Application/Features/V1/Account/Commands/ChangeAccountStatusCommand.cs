using Festpay.Onboarding.Application.Common.Exceptions;
using Festpay.Onboarding.Application.Interfaces.IRepositories;
using FluentValidation;
using MediatR;
using Entities = Festpay.Onboarding.Domain.Entities;

namespace Festpay.Onboarding.Application.Features.V1.Account.Commands;

public sealed record ChangeAccountStatusCommand(Guid Id, bool? DisableIntention) : IRequest<Result>;

public sealed class ChangeAccountStatusCommandValidator
    : AbstractValidator<ChangeAccountStatusCommand>
{
    public ChangeAccountStatusCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
    }
}

public sealed class ChangeAccountStatusCommandHandler(IAccountRepository repository)
    : IRequestHandler<ChangeAccountStatusCommand, Result>
{
    public async Task<Result> Handle(
        ChangeAccountStatusCommand request,
        CancellationToken cancellationToken
    )
    {
        var account = await repository.GetAccountWithTrack(request.Id, cancellationToken);
        if (account == null)
            throw new EntityDoesntExistException(nameof(Entities.Account));

        bool isDeactivated = account.DeactivatedUtc.HasValue;
        if (request.DisableIntention is null || (bool)request.DisableIntention != isDeactivated)
            account.EnableDisable();

        await repository.ConfirmModelChanges(cancellationToken);

        return Result.Ok();
    }
}
