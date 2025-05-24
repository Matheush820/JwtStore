using JwtStore.Core.Context.AccountContext.Entities;
using JwtStore.Core.Context.AccountContext.UseCases.Authenticate.Contracts;
using JwtStore.Core.Context.AccountContext.UseCases.Create;
using MediatR;

namespace JwtStore.Core.Context.AccountContext.UseCases.Authenticate;

public class Handler : IRequestHandler<Request, Response>
{
    private readonly IRepository _repository;

    public Handler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
    {
        #region Valida a requisição
        try
        {
            var res = Specification.Ensure(request);
            if (!res.IsValid)
                return new Response("Requisição inválida", 400, res.Notifications);
        }
        catch
        {
            return new Response("Não foi possível validar sua requisição", 500);
        }
        #endregion

        #region Recupera o usuário
        User? user;
        try
        {
            user = await _repository.GetUserByEmail(request.Email, cancellationToken);
            if (user is null)
                return new Response("Perfil não encontrado", 404);
        }
        catch
        {
            return new Response("Não foi possível recuperar seu perfil", 500);
        }
        #endregion

        #region Verifica a senha
        if (!user.Password.Challenge(request.Password))
            return new Response("Usuário ou senha inválidos", 400);
        #endregion

        #region Verifica status da conta
        try
        {
            if (!user.Email.Verification.IsActive)
                return new Response("Conta inativa", 400);
        }
        catch
        {
            return new Response("Não foi possível verificar o seu perfil", 500);
        }
        #endregion

        #region recupera os perfis do usuario
        try
        {
            user = await _repository.GetUserByEmail(request.Email, cancellationToken);
            if (user is null)
                return new Response("Foi possivel recuperar o seu usuario", 200);
        }
        catch
        {
            return new Response("Não foi possivel recuperar o seu usuario", 404);

        }
        #endregion

        #region Retorna os dados
        try
        {
            var data = new ResponseData(
    user.Id,
    user.Name,
    user.Email,
    user.Roles.Select(x => x.Name).ToArray()
    );
            return new Response("Retornado com sucesso", 200);
        }
        catch
        {
            return new Response("Não foi possível retornar os dados do usuário", 500);
        }
        #endregion
    }
}
