using Festpay.Onboarding.Application.Common.Exceptions;
using Festpay.Onboarding.Application.Interfaces.IRepositories;
using FluentValidation;
using MediatR;

namespace Festpay.Onboarding.Application.Features.V1.Account.Commands;

public sealed record ChangeAccountStatusCommand(Guid Id, bool? DisableIntention) : IRequest;

public sealed class ChangeAccountStatusCommandValidator
    : AbstractValidator<ChangeAccountStatusCommand>
{
    public ChangeAccountStatusCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
    }
}

public sealed class ChangeAccountStatusCommandHandler(IAccountRepository repository)
    : IRequestHandler<ChangeAccountStatusCommand>
{
    public async Task Handle(
        ChangeAccountStatusCommand request,
        CancellationToken cancellationToken
    )
    {
        // TODO: trocar por verify ou receber também a intenção do usuário
        var account = await repository.GetAccount(request.Id, cancellationToken);
        
        if (account == null)
            throw new EntityAlreadyExistsException(nameof(account));

        bool isDeactivated = account.DeactivatedUtc.HasValue;
        if (request.DisableIntention is null || (bool)request.DisableIntention != isDeactivated)
            account.EnableDisable();

        await repository.ConfirmModelChanges(cancellationToken);
    }
}
