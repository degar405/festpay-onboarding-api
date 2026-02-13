using Festpay.Onboarding.Domain.Exceptions;
using Festpay.Onboarding.Domain.Extensions;

namespace Festpay.Onboarding.Domain.Entities;

public class Account : EntityBase
{
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Phone { get; private set; } = string.Empty;
    public string Document { get; private set; } = string.Empty;
    public decimal Balance { get; private set; } = 0;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public override void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new RequiredFieldException(nameof(Name));

        if (!Document.IsValidDocument())
            throw new InvalidDocumentNumberException(Document);

        if (!Email.IsValidEmail())
            throw new InvalidEmailFormatException(Email);

        if (!Phone.IsValidPhone())
            throw new InvalidPhoneNumberException(Phone);
    }

    public static Account Create(string name, string document, string email, string phone)
    {
        var account = new Account
        {
            Name = name,
            Document = document,
            Email = email,
            Phone = phone
        };

        account.Validate();
        return account;
    }
}
