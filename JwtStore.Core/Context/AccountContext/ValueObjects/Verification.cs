using JwtStore.Core.Context.SharedContext.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtStore.Core.Context.AccountContext.ValueObjects;
public class Verification : ValueObject
{
    public Verification()
    {
        
    }
    public string Code { get; } = Guid.NewGuid().ToString("N")[0..6].ToUpper();
    public DateTime? ExpiresAt { get; private set; } = DateTime.UtcNow.AddMinutes(5);
    public DateTime? VerifiedAt { get; private set; } = null;
    public bool IsActive => VerifiedAt != null && ExpiresAt == null;


    public void Verify (string code)
    {
        if (IsActive)
            throw new Exception("Este item ja foi ativado");

        if (ExpiresAt < DateTime.UtcNow)
            throw new Exception("Este codigo ja expirou");

        if (!string.Equals(code.Trim(), Code.Trim(), StringComparison.CurrentCultureIgnoreCase))
            throw new Exception("Codigo de verificação invalido");

        ExpiresAt = null;
        VerifiedAt = DateTime.UtcNow;
    }
}
