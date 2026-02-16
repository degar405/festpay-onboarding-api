namespace Festpay.Onboarding.Application.Common.Constants;

public class ErrorMessageConstants
{
    public const string NotFound = "{0} not found";
    public const string EntityAlreadyExists = "{0} already exists";
    public const string EntityDoesntExist = "{0} does not exist";
    public const string OperationAlreadyPerformed = "Operation already performed for this {0}.";
    public const string InativeEntity = "{0} is deactivated.";
    public const string ConcurrentOperationDetected = "Another operation affected this {0} while your request was being processed. Please try again.";
}
