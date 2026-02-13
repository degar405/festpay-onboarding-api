using Festpay.Onboarding.Domain.Entities;
using Festpay.Onboarding.Domain.Exceptions;

namespace Festpay.Onboarding.Domain.Tests.Entities;

public class AccountTests
{
    [Fact]
    public void Should_Create_Account_When_Data_Is_Valid()
    {
        var account = Account.Create(
            "John Doe",
            "16670073607",
            "john.doe@example.com",
            "11999999999"
        );

        Assert.Equal("John Doe", account.Name);
        Assert.Equal("16670073607", account.Document);
        Assert.Equal("john.doe@example.com", account.Email);
        Assert.Equal("11999999999", account.Phone);
    }

    [Fact]
    public void Should_Throw_RequiredFieldException_When_Name_Is_Empty()
    {
        var exception = Assert.Throws<RequiredFieldException>(
            () =>
                Account.Create(
                    "",
                    "16670073607",
                    "john.doe@example.com",
                    "11999999999"
                )
        );

        Assert.Equal("Name", exception.FieldName);
    }

    [Fact]
    public void Should_Throw_InvalidDocumentNumberException_When_Document_Is_Invalid()
    {
        var invalidDocument = "00000000000";

        var exception = Assert.Throws<InvalidDocumentNumberException>(
            () =>
                Account.Create(
                    "John Doe",
                    invalidDocument,
                    "john.doe@example.com",
                    "11999999999"
                )
        );

        Assert.Equal(invalidDocument, exception.Document);
    }

    [Fact]
    public void Should_Throw_InvalidEmailFormatException_When_Email_Is_Invalid()
    {
        var invalidEmail = "john.doeexample.com";

        var exception = Assert.Throws<InvalidEmailFormatException>(
            () =>
                Account.Create(
                    "John Doe",
                    "16670073607",
                    invalidEmail,
                    "11999999999"
                )
        );

        Assert.Equal(invalidEmail, exception.Email);
    }

    [Fact]
    public void Should_Throw_InvalidPhoneNumberException_When_Phone_Is_Invalid()
    {
        var invalidPhone = "123";

        var exception = Assert.Throws<InvalidPhoneNumberException>(
            () =>
                Account.Create(
                    "John Doe",
                    "16670073607",
                    "john.doe@example.com",
                    invalidPhone
                )
        );

        Assert.Equal(invalidPhone, exception.Phone);
    }
}
