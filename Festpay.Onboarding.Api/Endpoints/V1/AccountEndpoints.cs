using Carter;
using Festpay.Onboarding.Api.Models;
using Festpay.Onboarding.Application.Features.V1.Account.Commands;
using Festpay.Onboarding.Application.Features.V1.Account.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Festpay.Onboarding.Api.Endpoints.V1
{
    public class AccountEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost($"{EndpointConstants.V1}{EndpointConstants.Account}",
                async ([FromServices] ISender sender, [FromBody] CreateAccountCommand command) =>
                {
                    var result = await sender.Send(command);
                    return Result.Ok(result);
                }
            )
            .WithTags(SwaggerTagsConstants.Account);

            app.MapPatch($"{EndpointConstants.V1}{EndpointConstants.Account}/{{id:guid}}",
                async ([FromServices] ISender sender, [FromRoute] Guid id) =>
                {
                    var command = new ChangeAccountStatusCommand(id, null);
                    await sender.Send(command);
                    return Result.Ok(null);
                }
            )
            .WithTags(SwaggerTagsConstants.Account);

            app.MapGet($"{EndpointConstants.V1}{EndpointConstants.Account}",
                async ([FromServices] ISender sender) =>
                {
                    var result = await sender.Send(new GetAccountsQuery());
                    return Result.Ok(result);
                }
            )
            .WithTags(SwaggerTagsConstants.Account);
        }
    }
}
