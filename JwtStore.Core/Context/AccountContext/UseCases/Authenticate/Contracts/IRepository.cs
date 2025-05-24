using JwtStore.Core.Context.AccountContext.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtStore.Core.Context.AccountContext.UseCases.Authenticate.Contracts;
public interface IRepository
{
    Task<User> GetUserByEmail(string email, CancellationToken cancellationToken);
}
