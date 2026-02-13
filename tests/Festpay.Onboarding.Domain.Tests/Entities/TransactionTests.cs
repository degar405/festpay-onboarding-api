using Festpay.Onboarding.Domain.Entities;
using Festpay.Onboarding.Domain.Exceptions;

namespace Festpay.Onboarding.Domain.Tests.Entities
{
    public class TransactionTests
    {
        [Fact]
        public void Should_Create_Transaction_When_Data_Is_Valid()
        {
            var source = Guid.NewGuid();
            var destination = Guid.NewGuid();
            var value = 100.50m;

            var tx = Transaction.Create(source, destination, value);

            Assert.Equal(source, tx.SourceAccountID);
            Assert.Equal(destination, tx.DestinationAccountID);
            Assert.Equal(value, tx.Value);
        }

        [Fact]
        public void Should_Throw_RequiredFieldException_When_SourceAccountID_Is_Empty()
        {
            var source = Guid.Empty;
            var destination = Guid.NewGuid();
            var value = 10m;

            var ex = Assert.Throws<RequiredFieldException>(() =>
                Transaction.Create(source, destination, value)
            );

            Assert.Equal(nameof(Transaction.SourceAccountID), ex.FieldName);
        }

        [Fact]
        public void Should_Throw_RequiredFieldException_When_DestinationAccountID_Is_Empty()
        {
            var source = Guid.NewGuid();
            var destination = Guid.Empty;
            var value = 10m;

            var ex = Assert.Throws<RequiredFieldException>(() =>
                Transaction.Create(source, destination, value)
            );

            Assert.Equal(nameof(Transaction.DestinationAccountID), ex.FieldName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Should_Throw_NumericalValueLessThanMinException_When_Value_Is_LessOrEqual_Zero(decimal invalidValue)
        {
            var source = Guid.NewGuid();
            var destination = Guid.NewGuid();

            var ex = Assert.Throws<NumericalValueLessThanMinException>(() =>
                Transaction.Create(source, destination, invalidValue)
            );

            Assert.Equal(nameof(Transaction.Value), ex.FieldName);
            Assert.Equal("0.00", ex.ComparisonValue);
        }

        [Fact]
        public void Should_Throw_TransactionDestinedToSourceAccountException_When_Source_Equals_Destination()
        {
            var id = Guid.NewGuid();
            var value = 50m;

            Assert.Throws<TransactionDestinedToSourceAccountException>(() =>
                Transaction.Create(id, id, value)
            );
        }
    }
}
