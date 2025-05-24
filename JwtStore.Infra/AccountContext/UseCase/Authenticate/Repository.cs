using JwtStore.Core.Context.AccountContext.Entities;
using JwtStore.Core.Context.AccountContext.UseCases.Authenticate.Contracts;
using JwtStore.Infra.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtStore.Infra.AccountContext.UseCase.Authenticate;
public class Repository : IRepository
{
    private readonly AppDbContext _context;

    public Repository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User> GetUserByEmail(string email, CancellationToken cancellationToken)
    {
        return await _context
            .Users
            .AsNoTracking()
            .Include(x => x.Roles)
            .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
    }
}
