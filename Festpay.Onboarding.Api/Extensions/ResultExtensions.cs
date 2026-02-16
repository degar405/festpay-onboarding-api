using Festpay.Onboarding.Api.Models;
using Festpay.Onboarding.Application.Common.Results;

public static class ResultExtensions
{
    public static IResult ToHttpResult(this Result result, string httpMethod)
    {
        if (result.Success)
            return MapSuccess(httpMethod);

        return result.ErrorType switch
        {
            ErrorTypeEnum.Validation =>
                Results.UnprocessableEntity(new ErrorResponseModel(result.Errors)),

            ErrorTypeEnum.NotFound =>
                Results.NotFound(new ErrorResponseModel(result.Errors)),

            ErrorTypeEnum.Conflict =>
                Results.Conflict(new ErrorResponseModel(result.Errors)),

            ErrorTypeEnum.Failure =>
                Results.BadRequest(new ErrorResponseModel(result.Errors)),

            _ =>
                Results.BadRequest(new ErrorResponseModel(result.Errors))
        };
    }

    public static IResult ToHttpResult<T>(this Result<T> result, string httpMethod)
    {
        if (result.Success)
            return MapSuccessWithData(httpMethod, result.Data);

        return ((Result)result).ToHttpResult(httpMethod);
    }

    private static IResult MapSuccess(string httpMethod)
    {
        return httpMethod.ToUpper() switch
        {
            "POST" => Results.Created(string.Empty, null),
            "PUT" => Results.NoContent(),
            "PATCH" => Results.NoContent(),
            "DELETE" => Results.NoContent(),
            _ => Results.Ok()
        };
    }

    private static IResult MapSuccessWithData<T>(string httpMethod, T? data)
    {
        return httpMethod.ToUpper() switch
        {
            "POST" => Results.Created(string.Empty, data),
            "PUT" => Results.NoContent(),
            "PATCH" => Results.NoContent(),
            "DELETE" => Results.NoContent(),
            _ => Results.Ok(data)
        };
    }
}
