using Festpay.Onboarding.Application.Common.Constants;
using Festpay.Onboarding.Application.Common.Results;
using Festpay.Onboarding.Application.Interfaces.IRepositories;
using Festpay.Onboarding.Domain.Extensions;
using FluentValidation;
using MediatR;
using Entities = Festpay.Onboarding.Domain.Entities;

namespace Festpay.Onboarding.Application.Features.V1.Account.Commands;

public sealed record CreateAccountCommand(
    string Name,
    string Document,
    string Email,
    string Phone
) : IRequest<Result<Guid>>;

public sealed class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");

        RuleFor(x => x.Document)
            .NotEmpty()
            .WithMessage("Document is required.")
            .Must(x => x.IsValidDocument())
            .WithMessage("Invalid document number.");

        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.Email))
            .WithMessage("Invalid email format.");

        RuleFor(x => x.Phone)
            .Must(x => x.IsValidPhone())
            .When(x => !string.IsNullOrWhiteSpace(x.Phone))
            .WithMessage("Invalid phone number.");
    }
}

public sealed class CreateAccountCommandHandler(IAccountRepository repository) : IRequestHandler<CreateAccountCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        CreateAccountCommand request,
        CancellationToken cancellationToken
    )
    {
        if (await repository.VerifyAccountExistence(request.Document, cancellationToken))
        {
            return Result<Guid>.Conflict(string.Format(ErrorMessageConstants.EntityAlreadyExists, nameof(Entities.Account)));
        }

        var account = Entities.Account.Create(
            request.Name,
            request.Document,
            request.Email,
            request.Phone
        );

        return await repository.CreateAccount(account, cancellationToken);
    }
}
