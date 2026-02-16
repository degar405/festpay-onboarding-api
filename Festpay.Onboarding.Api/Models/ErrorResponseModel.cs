namespace Festpay.Onboarding.Api.Models;

public class ErrorResponseModel
{
    public string ErrorMessage { get; set; } = "An unexpected error occurred.";
    public string[]? Errors { get; set; } = null;

    public ErrorResponseModel() { }

    public ErrorResponseModel(string[]? errors) { 
        ErrorMessage = "Look up the errors list and the documentation for more details.";
        Errors = errors;
    }

    private ErrorResponseModel(Exception exception) {
        ErrorMessage = exception.Message;
        Errors = [
            exception.GetType().Name,
            exception.StackTrace ?? string.Empty
        ];
    }

    public static ErrorResponseModel CreateDevelopmentErrorResponse(Exception exception)
    {
        return new ErrorResponseModel(exception); 
    }
}