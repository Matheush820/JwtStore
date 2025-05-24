namespace JwtStore.Core.AccountContext.ValueObjects;

using JwtStore.Core.Context.AccountContext.ValueObjects;
using JwtStore.Core.Context.SharedContext.Extensions;
using JwtStore.Core.Context.SharedContext.ValueObjects;
using System.Text.RegularExpressions;

public partial class Email : ValueObject
{
    protected Email()
    {
        
    }
    private const string Pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

    public Email(string address)
    {
        if (string.IsNullOrEmpty(address))
            throw new Exception("Email inválido");

        Address = address.Trim().ToLower();

        if (Address.Length < 5)
            throw new Exception("Email inválido");

        if (!EmailRegex().IsMatch(Address))
            throw new Exception("Email inválido");
    }


    public string Address { get; }
    public string Hash => Address.ToBase64();
    public static implicit operator string(Email email) => email.Address;

    public Verification Verification { get; private set; } = new();

    public void ResendVerification()
    {
        Verification = new Verification();
    }

    public static implicit operator Email(string address) => new Email(address);


    public override string ToString() => Address;


    [GeneratedRegex(Pattern)]
    private static partial Regex EmailRegex();
}
