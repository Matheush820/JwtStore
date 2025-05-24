using MediatR;

namespace JwtStore.Core.Context.AccountContext.UseCases.Create;
public record Request(
    string Name,
    string Email,
    string Password
) : IRequest<Response>;

