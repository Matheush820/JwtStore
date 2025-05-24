using JwtStore.Core.Context.AccountContext.Entities;
using JwtStore.Core.Context.AccountContext.UseCases.Create.Contracts;
using JwtStore.Infra.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtStore.Infra.AccountContext.UseCase.Create;
public class Repository : IRepository
{
    private readonly AppDbContext _context;

    public Repository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> AnyAsync(string email, CancellationToken cancellationToken)
    => await _context.Users
        .AsNoTracking()
        .AnyAsync(x => x.Email == email, cancellationToken: cancellationToken);

    public async Task<bool> SaveAsync(User user, CancellationToken cancellationToken)
    {
        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

}
