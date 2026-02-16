using Festpay.Onboarding.Application.Common.Constants;
using Festpay.Onboarding.Application.Common.Results;
using Festpay.Onboarding.Application.Features.V1.Account.Commands;
using Festpay.Onboarding.Application.Interfaces.IRepositories;
using Moq;
using Entities = Festpay.Onboarding.Domain.Entities;

namespace Festpay.Onboarding.Application.Tests.Features.V1.Account;

public class ChangeAccountStatusCommandHandlerTests
{
    [Fact]
    public void Validator_Should_Fail_When_Id_Is_Empty()
    {
        var validator = new ChangeAccountStatusCommandValidator();
        var command = new ChangeAccountStatusCommand(Guid.Empty, null);

        var result = validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(ChangeAccountStatusCommand.Id));
    }

    [Fact]
    public void Validator_Should_Pass_When_Id_Is_Not_Empty()
    {
        var validator = new ChangeAccountStatusCommandValidator();
        var command = new ChangeAccountStatusCommand(Guid.NewGuid(), null);

        var result = validator.Validate(command);

        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task Handler_Should_Toggle_When_DisableIntention_Is_Null()
    {
        // Arrange
        var account = Entities.Account.Create("Test", "12345678909", "test@example.com", "11999999999");
        var repoMock = new Mock<IAccountRepository>();
        repoMock
            .Setup(r => r.GetAccountWithTrack(account.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(account);
        repoMock
            .Setup(r => r.ConfirmModelChanges(It.IsAny<CancellationToken>(), It.IsAny<string>()))
            .Returns(Task.FromResult(Result.Ok()));

        var handler = new ChangeAccountStatusCommandHandler(repoMock.Object);
        var command = new ChangeAccountStatusCommand(account.Id, null);

        // Act - disable
        await handler.Handle(command, CancellationToken.None);

        // Assert disabled & confirm called once
        Assert.NotNull(account.DeactivatedUtc);
        repoMock.Verify(r => r.ConfirmModelChanges(It.IsAny<CancellationToken>(), It.IsAny<string>()), Times.Once);

        // Act - enable (call again)
        await handler.Handle(command, CancellationToken.None);

        // Assert enabled & confirm called twice total
        Assert.Null(account.DeactivatedUtc);
        repoMock.Verify(r => r.ConfirmModelChanges(It.IsAny<CancellationToken>(), It.IsAny<string>()), Times.Exactly(2));
    }

    [Fact]
    public async Task Handler_Should_Return_Result_NotFound_When_Account_Not_Found()
    {
        // Arrange
        var repoMock = new Mock<IAccountRepository>();
        repoMock
            .Setup(r => r.GetAccountWithTrack(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Entities.Account?)null);

        var handler = new ChangeAccountStatusCommandHandler(repoMock.Object);
        var command = new ChangeAccountStatusCommand(Guid.NewGuid(), null);

        // Act & Assert
        var result = await handler.Handle(command, CancellationToken.None);
        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.Equal(ErrorTypeEnum.NotFound, result.ErrorType);
        Assert.Equal(string.Format(ErrorMessageConstants.NotFound, nameof(Entities.Account)), result.Errors.FirstOrDefault());
        repoMock.Verify(r => r.ConfirmModelChanges(It.IsAny<CancellationToken>(), It.IsAny<string>()), Times.Never);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Handler_Should_Not_Toggle_When_DisableIntention_Equals_Current_State(bool currentlyDeactivated)
    {
        // Arrange
        var account = Entities.Account.Create("Test", "12345678909", "test@example.com", "11999999999");
        if (currentlyDeactivated)
            account.EnableDisable(); // set DeactivatedUtc

        var repoMock = new Mock<IAccountRepository>();
        repoMock
            .Setup(r => r.GetAccountWithTrack(account.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(account);
        repoMock
            .Setup(r => r.ConfirmModelChanges(It.IsAny<CancellationToken>(), It.IsAny<string>()))
            .Returns(Task.FromResult(Result.Ok()));

        var handler = new ChangeAccountStatusCommandHandler(repoMock.Object);
        var command = new ChangeAccountStatusCommand(account.Id, currentlyDeactivated);

        var before = account.DeactivatedUtc;

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert no change and ConfirmModelChanges called once
        Assert.Equal(before, account.DeactivatedUtc);
        repoMock.Verify(r => r.ConfirmModelChanges(It.IsAny<CancellationToken>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Handler_Should_Toggle_When_DisableIntention_Is_Different_From_Current_State()
    {
        // Arrange: account currently enabled
        var account = Entities.Account.Create("Test", "12345678909", "test@example.com", "11999999999");
        Assert.Null(account.DeactivatedUtc);

        var repoMock = new Mock<IAccountRepository>();
        repoMock
            .Setup(r => r.GetAccountWithTrack(account.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(account);
        repoMock
            .Setup(r => r.ConfirmModelChanges(It.IsAny<CancellationToken>(), It.IsAny<string>()))
            .Returns(Task.FromResult(Result.Ok()));

        var handler = new ChangeAccountStatusCommandHandler(repoMock.Object);

        // Act: ask to disable (different from current enabled state)
        var commandDisable = new ChangeAccountStatusCommand(account.Id, true);
        await handler.Handle(commandDisable, CancellationToken.None);

        // Assert disabled and saved
        Assert.NotNull(account.DeactivatedUtc);
        repoMock.Verify(r => r.ConfirmModelChanges(It.IsAny<CancellationToken>(), It.IsAny<string>()), Times.Once);

        // Act: ask to enable (different from current deactivated state)
        var commandEnable = new ChangeAccountStatusCommand(account.Id, false);
        await handler.Handle(commandEnable, CancellationToken.None);

        // Assert enabled and saved again
        Assert.Null(account.DeactivatedUtc);
        repoMock.Verify(r => r.ConfirmModelChanges(It.IsAny<CancellationToken>(), It.IsAny<string>()), Times.Exactly(2));
    }
}