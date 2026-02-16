using Festpay.Onboarding.Application.Common.Constants;
using Festpay.Onboarding.Application.Common.Results;
using Festpay.Onboarding.Application.Features.V1.Account.Commands;
using Festpay.Onboarding.Application.Interfaces.IRepositories;
using Moq;
using Entities = Festpay.Onboarding.Domain.Entities;

namespace Festpay.Onboarding.Application.Tests.Features.V1.Account;

public class CreateAccountCommandTests
{
    // -------- Validator tests --------

    [Fact]
    public void Validator_Should_Fail_When_Name_Is_Empty()
    {
        var validator = new CreateAccountCommandValidator();
        var command = new CreateAccountCommand("", "16670073607", "test@example.com", "11999999999");

        var result = validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateAccountCommand.Name));
    }

    [Fact]
    public void Validator_Should_Fail_When_Document_Is_Empty()
    {
        var validator = new CreateAccountCommandValidator();
        var command = new CreateAccountCommand("Name", "", "test@example.com", "11999999999");

        var result = validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateAccountCommand.Document));
    }

    [Fact]
    public void Validator_Should_Fail_When_Document_Is_Invalid()
    {
        var validator = new CreateAccountCommandValidator();
        var command = new CreateAccountCommand("Name", "00000000000", "test@example.com", "11999999999");

        var result = validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateAccountCommand.Document));
    }

    [Fact]
    public void Validator_Should_Fail_When_Email_Is_Invalid()
    {
        var validator = new CreateAccountCommandValidator();
        var command = new CreateAccountCommand("Name", "16670073607", "invalid-email", "11999999999");

        var result = validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateAccountCommand.Email));
    }

    [Fact]
    public void Validator_Should_Fail_When_Phone_Is_Invalid()
    {
        var validator = new CreateAccountCommandValidator();
        var command = new CreateAccountCommand("Name", "16670073607", "test@example.com", "123");

        var result = validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateAccountCommand.Phone));
    }

    [Fact]
    public void Validator_Should_Pass_For_Valid_Input()
    {
        var validator = new CreateAccountCommandValidator();
        var command = new CreateAccountCommand("John Doe", "16670073607", "john.doe@example.com", "11999999999");

        var result = validator.Validate(command);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validator_Should_Pass_When_Email_And_Phone_Are_Empty()
    {
        var validator = new CreateAccountCommandValidator();
        var command = new CreateAccountCommand("John Doe", "16670073607", "", "");

        var result = validator.Validate(command);

        Assert.True(result.IsValid);
    }

    // -------- Handler tests --------

    [Fact]
    public async Task Handler_Should_Return_Conflict_When_Document_Already_Exists()
    {
        // Arrange
        var repoMock = new Mock<IAccountRepository>();
        repoMock
            .Setup(r => r.VerifyAccountExistence(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var handler = new CreateAccountCommandHandler(repoMock.Object);
        var command = new CreateAccountCommand("Name", "16670073607", "test@example.com", "11999999999");

        // Act & Assert
        var result = await handler.Handle(command, CancellationToken.None);
        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.Equal(ErrorTypeEnum.Conflict, result.ErrorType);
        Assert.Equal(string.Format(ErrorMessageConstants.EntityAlreadyExists, nameof(Entities.Account)), result.Errors.FirstOrDefault());

        repoMock.Verify(r => r.CreateAccount(It.IsAny<Entities.Account>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handler_Should_Create_Account_When_Data_Is_Valid()
    {
        // Arrange
        var expectedId = Result<Guid>.Ok(Guid.NewGuid());
        var repoMock = new Mock<IAccountRepository>();
        repoMock
            .Setup(r => r.VerifyAccountExistence(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        repoMock
            .Setup(r => r.CreateAccount(It.IsAny<Entities.Account>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedId);

        var handler = new CreateAccountCommandHandler(repoMock.Object);
        var command = new CreateAccountCommand("John Doe", "16670073607", "john.doe@example.com", "11999999999");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(expectedId.Data, result?.Data);
        repoMock.Verify(r => r.VerifyAccountExistence(command.Document, It.IsAny<CancellationToken>()), Times.Once);

        repoMock.Verify(r =>
            r.CreateAccount(
                It.Is<Entities.Account>(a =>
                    a.Name == command.Name &&
                    a.Document == command.Document &&
                    a.Email == command.Email &&
                    a.Phone == command.Phone
                ),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
    }
}