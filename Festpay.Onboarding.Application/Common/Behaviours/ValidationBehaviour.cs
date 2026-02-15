using Festpay.Onboarding.Application.Common.Results;
using FluentValidation;
using MediatR;
using System.Reflection;

namespace Festpay.Onboarding.Application.Common.Behaviours;

public class ValidationBehaviour<TRequest, TUnit>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TUnit>
    where TRequest : IRequest<TUnit>
    where TUnit : Result
{
    public async Task<TUnit> Handle(
        TRequest request,
        RequestHandlerDelegate<TUnit> next,
        CancellationToken cancellationToken
    )
    {
        var validationErrors = validators
            .Select(v => v.Validate(request))
            .SelectMany(result => result.Errors)
            .Where(f => f != null)
            .Select(f => f.ErrorMessage)
            .ToList();

        if (validationErrors.Count == 0)
            return await next();

        return CreateValidationResult<TUnit>(validationErrors.ToArray());
    }

    private static T CreateValidationResult<T>(string[] errors)
        where T : Result
    {
        if (typeof(T).IsGenericType)
        {
            var resultType = typeof(T);
            var ValidationMethod = resultType
                .GetMethod("Validation", BindingFlags.Public | BindingFlags.Static);

            return (T)ValidationMethod!
                .Invoke(null, [errors])!;
        }

        return (T)(object)Result.Validation(errors);
    }
}
