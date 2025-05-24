using JwtStore.Core.AccountContext.ValueObjects;
using JwtStore.Core.Context.AccountContext.Entities;
using JwtStore.Core.Context.AccountContext.UseCases.Create.Contracts;
using JwtStore.Core.Context.AccountContext.ValueObjects;
using MediatR;

namespace JwtStore.Core.Context.AccountContext.UseCases.Create;
public class Handler : IRequestHandler<Request, Response>

{
    private readonly IRepository _repository;
    private readonly IService _service;

    public Handler(IRepository repository, IService service)
    {
        _repository = repository;
        _service = service;
    }

    public async Task<Response> Handle(
        Request request, 
        CancellationToken cancellationToken)
    {
        #region valida a requisição
        try
        {
            var res = Specification.Ensure(request);
            if (!res.IsValid)
                return new Response("requisição invalida", 400, res.Notifications);
        }
        catch (Exception ex)
        {
            return new Response("requisição invalida", 500, null);
        }

        #endregion

        #region Gerar os objetos

        Email email;
        Password password;
        User user;

        try
        {
            email = new Email(request.Email);
            password = new Password(request.Password);
            user = new User(request.Name,email, password);

        }
        catch(Exception ex)
        {
            return new Response(ex.Message, 400);
        }

        #endregion
        #region Verifica se o usuario existe no banco

        try
        {
            var exists = await _repository.AnyAsync(request.Email, cancellationToken);

            if (exists)
                return new Response("Este Email já esta em uso", 400);
        }
        catch
        {
            return new Response("Falha ao cadastrar email", 500);

        }
        #endregion
        #region
        try
        {
            await _repository.SaveAsync(user, cancellationToken);
        }
        catch
        {
            return new Response("Falha ao persistir dados", 500);

        }
        #endregion

        #region
        try
        {
            await _service.SendVerificationEmailAsync(user, cancellationToken);
        }
        catch
        {
            return new Response("Falha ao verificar o email", 500);

        }
        #endregion

        return new Response("Conta criada", new ResponseData(user.Id, user.Name, user.Email));
    }
}
