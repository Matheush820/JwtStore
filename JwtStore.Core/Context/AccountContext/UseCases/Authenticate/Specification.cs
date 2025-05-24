using Flunt.Notifications;
using Flunt.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtStore.Core.Context.AccountContext.UseCases.Authenticate;
public class Specification
{
    public static Contract<Notification> Ensure(Request request)
        => new Contract<Notification>()
        .Requires()
        .IsLowerThan(request.Password.Length, 40, "Password")
        .IsGreaterThan(request.Password.Length, 8, "Password")
        .IsEmail(request.Email, "Email", "Email invalido");
}
