using Carter;
using Festpay.Onboarding.Api.Models;
using Festpay.Onboarding.Application.Features.V1.Transaction.Commands;
using Festpay.Onboarding.Application.Features.V1.Transaction.Queries;
using MediatR;

namespace Festpay.Onboarding.Api.Endpoints.V1;

public class TransactionEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost($"{EndpointConstants.V1}{EndpointConstants.Transaction}",
            async (HttpContext context, ISender sender, CreateTransactionCommand command) =>
            {
                var result = await sender.Send(command);
                return result.ToHttpResult(context.Request.Method);
            }
        )
        .WithTags(SwaggerTagsConstants.Transaction);

        app.MapPatch($"{EndpointConstants.V1}{EndpointConstants.Transaction}/{{id:guid}}/cancel",
            async (HttpContext context, ISender sender, Guid id) =>
            {
                var command = new CancelTransactionCommand(id);
                var result = await sender.Send(command);
                return result.ToHttpResult(context.Request.Method);
            }
        )
        .WithTags(SwaggerTagsConstants.Transaction);

        app.MapGet($"{EndpointConstants.V1}{EndpointConstants.Transaction}",
            async (HttpContext context, ISender sender) =>
            {
                var result = await sender.Send(new GetTransactionsQuery());
                return result.ToHttpResult(context.Request.Method);
            }
        )
        .Produces<ICollection<GetTransactionsQueryResponse>>(StatusCodes.Status200OK)
        .Produces<ErrorResponseModel>(StatusCodes.Status500InternalServerError)
        .WithTags(SwaggerTagsConstants.Transaction);

        app.MapGet($"{EndpointConstants.V1}{EndpointConstants.Transaction}/{{id:guid}}",
            async (HttpContext context, ISender sender, Guid id) =>
            {
                var result = await sender.Send(new GetTransactionQuery(id));
                return result.ToHttpResult(context.Request.Method);
            }
        )
        .Produces<GetTransactionsQueryResponse>(StatusCodes.Status200OK)
        .Produces<ErrorResponseModel>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponseModel>(StatusCodes.Status422UnprocessableEntity)
        .Produces<ErrorResponseModel>(StatusCodes.Status500InternalServerError)
        .WithTags(SwaggerTagsConstants.Transaction);
    }
}
