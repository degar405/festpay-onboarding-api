namespace Festpay.Onboarding.Application.Common.Results;
public class Result<T> : Result
{
    private Result(T data)
        : base(true)
    {
        Data = data;
    }

    private Result(ErrorTypeEnum type, string[] errors)
        : base(false, type, errors)
    {
    }

    public T? Data { get; }

    public static Result<T> Ok(T data)
        => new(data);

    public static new Result<T> Validation(params string[] errors)
        => new(ErrorTypeEnum.Validation, errors);

    public static new Result<T> NotFound(params string[] errors)
        => new(ErrorTypeEnum.NotFound, errors);

    public static new Result<T> Conflict(params string[] errors)
        => new(ErrorTypeEnum.Conflict, errors);

    public static new Result<T> Failure(params string[] errors)
        => new(ErrorTypeEnum.Failure, errors);
}