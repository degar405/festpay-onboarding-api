using Carter;
using Festpay.Onboarding.Api.Models;
using Festpay.Onboarding.Application.Features.V1.Account.Commands;
using Festpay.Onboarding.Application.Features.V1.Account.Queries;
using MediatR;

namespace Festpay.Onboarding.Api.Endpoints.V1;

public class AccountEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost($"{EndpointConstants.V1}{EndpointConstants.Account}",
            async (HttpContext context, ISender sender, CreateAccountCommand command) =>
            {
                var result = await sender.Send(command);
                return result.ToHttpResult(context.Request.Method);
            }
        )
        .Produces<Guid>(StatusCodes.Status201Created)
        .Produces<ErrorResponseModel>(StatusCodes.Status422UnprocessableEntity)
        .Produces<ErrorResponseModel>(StatusCodes.Status409Conflict)
        .Produces<ErrorResponseModel>(StatusCodes.Status500InternalServerError)
        .WithTags(SwaggerTagsConstants.Account);

        app.MapPatch($"{EndpointConstants.V1}{EndpointConstants.Account}/{{id:guid}}/status",
            async (HttpContext context, ISender sender, Guid id, bool? deactivateIntention) =>
            {
                var command = new ChangeAccountStatusCommand(id, deactivateIntention);
                var result = await sender.Send(command);
                return result.ToHttpResult(context.Request.Method);
            }
        )
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ErrorResponseModel>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponseModel>(StatusCodes.Status409Conflict)
        .Produces<ErrorResponseModel>(StatusCodes.Status422UnprocessableEntity)
        .Produces<ErrorResponseModel>(StatusCodes.Status500InternalServerError)
        .WithTags(SwaggerTagsConstants.Account);

        app.MapGet($"{EndpointConstants.V1}{EndpointConstants.Account}",
            async (HttpContext context, ISender sender) =>
            {
                var result = await sender.Send(new GetAccountsQuery());
                return result.ToHttpResult(context.Request.Method);
            }
        )
        .Produces<ICollection<GetAccountsQueryResponse>>(StatusCodes.Status200OK)
        .Produces<ErrorResponseModel>(StatusCodes.Status500InternalServerError)
        .WithTags(SwaggerTagsConstants.Account);
    }
}