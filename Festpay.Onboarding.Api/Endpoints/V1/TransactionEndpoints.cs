using Carter;
using Festpay.Onboarding.Api.Models;
using Festpay.Onboarding.Application.Features.V1.Transaction.Queries;
using Festpay.Onboarding.Application.Features.V1.Transaction.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Festpay.Onboarding.Api.Endpoints.V1
{
    public class TransactionEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost($"{EndpointConstants.V1}{EndpointConstants.Transaction}",
                async ([FromServices] ISender sender, [FromBody] CreateTransactionCommand command) =>
                {
                    var result = await sender.Send(command);
                    return Result.Ok(result);
                }
            )
            .WithTags(SwaggerTagsConstants.Transaction);

            app.MapPatch($"{EndpointConstants.V1}{EndpointConstants.Transaction}/{{id:guid}}/cancel",
                async ([FromServices] ISender sender, [FromRoute] Guid id) =>
                {
                    var command = new CancelTransactionCommand(id);
                    await sender.Send(command);
                    return Result.Ok(null);
                }
            )
            .WithTags(SwaggerTagsConstants.Transaction);

            app.MapGet($"{EndpointConstants.V1}{EndpointConstants.Transaction}",
                async ([FromServices] ISender sender) =>
                {
                    var result = await sender.Send(new GetTransactionsQuery());
                    return Result.Ok(result);
                }
            )
            .WithTags(SwaggerTagsConstants.Transaction);

            app.MapGet($"{EndpointConstants.V1}{EndpointConstants.Transaction}/{{id:guid}}",
                async ([FromServices] ISender sender, [FromRoute] Guid id) =>
                {
                    var result = await sender.Send(new GetTransactionQuery(id));
                    return Result.Ok(result);
                }
            )
            .WithTags(SwaggerTagsConstants.Transaction);
        }
    }
}
