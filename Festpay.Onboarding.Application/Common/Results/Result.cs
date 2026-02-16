using Festpay.Onboarding.Application.Common.Results;

public class Result
{
    protected Result(
        bool success,
        ErrorTypeEnum? errorType = null,
        string[]? errors = null)
    {
        Success = success;
        ErrorType = errorType;
        Errors = errors ?? [];
    }

    public bool Success { get; }

    public bool IsFailure => !Success;

    public ErrorTypeEnum? ErrorType { get; }

    public string[] Errors { get; }

    public static Result Ok()
        => new(true);

    public static Result Validation(params string[] errors)
        => new(false, ErrorTypeEnum.Validation, errors);

    public static Result NotFound(params string[] errors)
        => new(false, ErrorTypeEnum.NotFound, errors);

    public static Result Conflict(params string[] errors)
        => new(false, ErrorTypeEnum.Conflict, errors);

    public static Result Failure(params string[] errors)
        => new(false, ErrorTypeEnum.Failure, errors);
}