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
        Assert.Equal(0m, account.Balance);
        Assert.NotEqual(default, account.CreatedAt);
    }

    [Fact]
    public void Should_Create_Account_With_Valid_Cnpj()
    {
        var validCnpj = "11222333000181";

        var account = Account.Create(
            "Empresa Ltda",
            validCnpj,
            "contato@empresa.com",
            "11999999999"
        );

        Assert.Equal("Empresa Ltda", account.Name);
        Assert.Equal(validCnpj, account.Document);
        Assert.Equal("contato@empresa.com", account.Email);
        Assert.Equal("11999999999", account.Phone);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Should_Throw_RequiredFieldException_When_Name_Is_Null_Or_Whitespace(string name)
    {
        var exception = Assert.Throws<RequiredFieldException>(
            () => Account.Create(
                name!,
                "16670073607",
                "john.doe@example.com",
                "11999999999"
            )
        );

        Assert.Equal("Name", exception.FieldName);
    }

    [Theory]
    [InlineData("00000000000")]
    [InlineData("00000000000000")]
    [InlineData("123")]
    public void Should_Throw_InvalidDocumentNumberException_For_Invalid_Documents(string invalidDocument)
    {
        var exception = Assert.Throws<InvalidDocumentNumberException>(
            () => Account.Create(
                "John Doe",
                invalidDocument,
                "john.doe@example.com",
                "11999999999"
            )
        );

        Assert.Equal(invalidDocument, exception.Document);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("john.doeexample.com")]
    [InlineData("john@doe@domain.com")]
    public void Should_Throw_InvalidEmailFormatException_For_Invalid_Emails(string invalidEmail)
    {
        var exception = Assert.Throws<InvalidEmailFormatException>(
            () => Account.Create(
                "John Doe",
                "16670073607",
                invalidEmail,
                "11999999999"
            )
        );

        Assert.Equal(invalidEmail, exception.Email);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("123")]          
    [InlineData("1199999999")]   
    [InlineData("119999999999")] 
    public void Should_Throw_InvalidPhoneNumberException_For_Invalid_Phones(string invalidPhone)
    {
        var exception = Assert.Throws<InvalidPhoneNumberException>(
            () => Account.Create(
                "John Doe",
                "16670073607",
                "john.doe@example.com",
                invalidPhone
            )
        );

        Assert.Equal(invalidPhone, exception.Phone);
    }

    [Fact]
    public void Should_Create_Account_When_Phone_Is_Valid_With_Parentheses()
    {
        var phoneWithParens = "(11999999999)";

        var account = Account.Create(
            "John Doe",
            "16670073607",
            "john.doe@example.com",
            phoneWithParens
        );

        Assert.Equal(phoneWithParens, account.Phone);
    }
}
