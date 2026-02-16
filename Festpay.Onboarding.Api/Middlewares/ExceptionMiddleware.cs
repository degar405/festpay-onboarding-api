using Festpay.Onboarding.Api.Models;
using Festpay.Onboarding.Application.Common.Exceptions;
using Festpay.Onboarding.Domain.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;

namespace Festpay.Onboarding.Api.Middlewares;

public class ExceptionMiddleware(
    RequestDelegate next,
    ILogger<ExceptionMiddleware> logger,
    IWebHostEnvironment environment)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var response = context.Response;

        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            response.ContentType = "application/json";

            response.StatusCode = ex switch
            {
                DomainException => StatusCodes.Status400BadRequest,
                ApplicationExceptions => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError,
            };

            if (response.StatusCode == (int)HttpStatusCode.InternalServerError)
            {
                logger.LogError(ex, "Unhandled exception occurred: {Message}", ex.Message);
            }

            var r = BuildResponse(ex, response.StatusCode);

            await response.WriteAsync(
                JsonConvert.SerializeObject(
                    r,
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        NullValueHandling = NullValueHandling.Ignore
                    }
                )
            );
        }
    }

    private ErrorResponseModel BuildResponse(Exception ex, int statusCode)
    {
        if (environment.IsDevelopment())
            return ErrorResponseModel.CreateDevelopmentErrorResponse(ex);

        if (statusCode == StatusCodes.Status500InternalServerError)
            return new ErrorResponseModel();

        return new ErrorResponseModel
        {
            ErrorMessage = ex.Message
        };
    }
}