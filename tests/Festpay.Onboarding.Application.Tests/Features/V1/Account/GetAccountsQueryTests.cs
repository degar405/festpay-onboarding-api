using Moq;
using Festpay.Onboarding.Application.Features.V1.Account.Queries;
using Festpay.Onboarding.Application.Interfaces.IRepositories;
using Entities = Festpay.Onboarding.Domain.Entities;

namespace Festpay.Onboarding.Application.Tests.Features.V1.Account;

public class GetAccountsQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Mapped_Accounts_When_Repository_Returns_Accounts()
    {
        // Arrange
        var account1 = Entities.Account.Create("Alice", "16670073607", "alice@example.com", "11999999999");
        var account2 = Entities.Account.Create("Empresa", "11222333000181", "empresa@example.com", "11988888888");
        // deactivate account2 to have a DeactivatedUtc value
        account2.EnableDisable();

        var accounts = new List<Entities.Account> { account1, account2 };

        var repoMock = new Mock<IAccountRepository>();
        repoMock
            .Setup(r => r.GetAccounts(It.IsAny<CancellationToken>()))
            .ReturnsAsync(accounts);

        var handler = new GetAccountsQueryHandler(repoMock.Object);

        // Act
        var result = await handler.Handle(new GetAccountsQuery(), CancellationToken.None);

        // Assert
        var dataResult = result?.Data;
        Assert.NotNull(dataResult);
        Assert.Equal(2, dataResult.Count);

        var resList = dataResult.ToList();

        // Validate first account mapping
        Assert.Equal(account1.Id, resList[0].Id);
        Assert.Equal(account1.Name, resList[0].Name);
        Assert.Equal(account1.Document, resList[0].Document);
        Assert.Equal(account1.Email, resList[0].Email);
        Assert.Equal(account1.Phone, resList[0].Phone);
        Assert.Equal(account1.CreatedUtc, resList[0].CreatedAt);
        Assert.Equal(account1.DeactivatedUtc, resList[0].DeactivatedAt);
        Assert.Equal(account1.Balance, resList[0].Balance);

        // Validate second account mapping (including DeactivatedUtc)
        Assert.Equal(account2.Id, resList[1].Id);
        Assert.Equal(account2.Name, resList[1].Name);
        Assert.Equal(account2.Document, resList[1].Document);
        Assert.Equal(account2.Email, resList[1].Email);
        Assert.Equal(account2.Phone, resList[1].Phone);
        Assert.Equal(account2.CreatedUtc, resList[1].CreatedAt);
        Assert.Equal(account2.DeactivatedUtc, resList[1].DeactivatedAt);
        Assert.Equal(account2.Balance, resList[1].Balance);

        repoMock.Verify(r => r.GetAccounts(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Return_EmptyCollection_When_Repository_Returns_No_Accounts()
    {
        // Arrange
        var repoMock = new Mock<IAccountRepository>();
        repoMock
            .Setup(r => r.GetAccounts(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Entities.Account>());

        var handler = new GetAccountsQueryHandler(repoMock.Object);

        // Act
        var result = await handler.Handle(new GetAccountsQuery(), CancellationToken.None);

        // Assert
        var resultData = result?.Data;
        Assert.NotNull(resultData);
        Assert.Empty(resultData);
        repoMock.Verify(r => r.GetAccounts(It.IsAny<CancellationToken>()), Times.Once);
    }
}