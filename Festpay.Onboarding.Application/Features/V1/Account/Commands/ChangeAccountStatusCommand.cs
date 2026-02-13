using Festpay.Onboarding.Application.Common.Exceptions;
using Festpay.Onboarding.Infra.Context;
using FluentValidation;
using MediatR;

namespace Festpay.Onboarding.Application.Features.V1.Account.Commands;

public sealed record ChangeAccountStatusCommand(Guid Id) : IRequest<bool>;

public sealed class ChangeAccountStatusCommandValidator
    : AbstractValidator<ChangeAccountStatusCommand>
{
    public ChangeAccountStatusCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
    }
}

public sealed class ChangeAccountStatusCommandHandler(FestpayContext dbContext)
    : IRequestHandler<ChangeAccountStatusCommand, bool>
{
    public async Task<bool> Handle(
        ChangeAccountStatusCommand request,
        CancellationToken cancellationToken
    )
    {
        var account =
             await dbContext.Accounts.FindAsync(request.Id, cancellationToken)
             ?? throw new NotFoundException("Conta");

        account.EnableDisable();

        return await dbContext.SaveChangesAsync(cancellationToken) > 0;
    }
}
