using Festpay.Onboarding.Application.Common.Constants;

namespace Festpay.Onboarding.Application.Common.Exceptions;

public class ApplicationExceptions : Exception
{
    public ApplicationExceptions(string message)
        : base(message) { }

    public ApplicationExceptions(string message, Exception innerException)
        : base(message, innerException) { }
}

public class NotFoundException(string entityName)
    : ApplicationExceptions(string.Format(ErrorMessageConstants.NotFound, entityName))
{ }

public class EntityDoesntExistException(string entityName)
    : ApplicationExceptions(
        string.Format(ErrorMessageConstants.EntityDoesntExist, entityName)
    )
{ }

public class OperationAlreadyPerformedException(string entityName)
    : ApplicationExceptions(
        string.Format(ErrorMessageConstants.OperationAlreadyPerformed, entityName)
    )
{ }

public class InactiveEntityException(string entityName)
    : ApplicationExceptions(
        string.Format(ErrorMessageConstants.InativeEntityException, entityName)
    )
{ }