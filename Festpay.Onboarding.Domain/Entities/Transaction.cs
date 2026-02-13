using Festpay.Onboarding.Domain.Exceptions;

namespace Festpay.Onboarding.Domain.Entities
{
    public class Transaction : EntityBase
    {
        public Guid SourceAccountID { get; private set; }
        public Guid DestinationAccountID { get; private set; }
        public decimal Value { get; private set; }
        public bool IsCanceled { get; private set; }

        public override void Validate()
        {
            if (SourceAccountID == Guid.Empty)
                throw new RequiredFieldException(nameof(SourceAccountID));

            if (DestinationAccountID == Guid.Empty)
                throw new RequiredFieldException(nameof(DestinationAccountID));

            if (Value <= 0)
                throw new NumericalValueLessThanMinException(nameof(Value), "0.00");

            if (SourceAccountID == DestinationAccountID)
                throw new TransactionDestinedToSourceAccountException();
        }

        public static Transaction Create(Guid sourceAccountID, Guid destinationAccountID, decimal value)
        {
            var transaction = new Transaction
            {
                SourceAccountID = sourceAccountID,
                DestinationAccountID = destinationAccountID,
                Value = value,
                IsCanceled = false
            };

            transaction.Validate();
            return transaction;
        }
    }
}
