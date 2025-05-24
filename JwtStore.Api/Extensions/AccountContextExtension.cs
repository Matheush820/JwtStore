using JwtStore.Core.Context.AccountContext.UseCases.Create;
using JwtStore.Core.Context.AccountContext.UseCases.Create.Contracts;
using JwtStore.Infra.AccountContext.UseCase.Create;
using MediatR;

namespace JwtStore.Api.Extensions;

public static class AccountContextExtension
{
    public static void AddAccountContext(this WebApplicationBuilder builder)
    {
        #region Create

        builder.Services.AddTransient<IRepository, Repository>();
        builder.Services.AddTransient<IService, Service>();

        #endregion

        #region Authenticate

        builder.Services.AddTransient<IRepository, Repository>();

        #endregion
    }

    public static void AddAccountEndpoints(this WebApplication app)
    {
        #region Create
        app.MapPost("api/v1/users", async (
            Request request,
            IRequestHandler<Request, Response> handler) =>
        {
            var result = await handler.Handle(request, new CancellationToken());
            return result.IsSuccess
                ? Results.Created($"/api/v1/users/{result.Data?.Id}", result)
                : Results.Json(result, statusCode: result.Status);
        });
        #endregion

        #region Authenticate
        app.MapPost("api/v1/authenticate", async (
            Request request,
            IRequestHandler<Request, Response> handler) =>
        {
            var result = await handler.Handle(request, new CancellationToken());
            return result.IsSuccess
                ? Results.Ok(result)
                : Results.Json(result, statusCode: result.Status);
        }).RequireAuthorization();
        #endregion
    }

}
