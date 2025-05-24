using Flunt.Notifications;
using Flunt.Validations;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtStore.Core.Context.AccountContext.UseCases.Create;
public static class Specification
{
    public static Contract<Notification> Ensure(Request request)
        => new Contract<Notification>()
        .Requires()
        .IsLowerThan(request.Name.Length, 160, "FirstName")
        .IsGreaterThan(request.Name.Length, 3, "FirstName")
        .IsLowerThan(request.Password.Length, 40, "Password")
        .IsGreaterThan(request.Password.Length, 8, "Password")
        .IsEmail(request.Email, "Email", "Email invalido");
}

