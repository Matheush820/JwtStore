using JwtStore.Core.Configurations;
using JwtStore.Core.Context.AccountContext.UseCases.Create;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtStore.Api.Extensions;

public static class JwtExtension
{
    public static string Generate(ResponseData data)
    {
        var handler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(Configuration.Secrets.JwtPrivateKey);
        var credentials = new SigningCredentials(new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = GenerateClaims(data),
            Expires = DateTime.UtcNow.AddHours(8),
            SigningCredentials = credentials
        };

        var token = handler.CreateToken(tokenDescriptor);
        return handler.WriteToken(token);
    }

    private static ClaimsIdentity GenerateClaims(ResponseData user)
    {
        var ci = new ClaimsIdentity();

        ci.AddClaim(new Claim("Id", user.Id.ToString()));
        ci.AddClaim(new Claim(ClaimTypes.GivenName, user.Name));
        ci.AddClaim(new Claim(ClaimTypes.Name, user.Email));

        // Supondo que Roles seja uma string separada por vírgula, como "Admin,User"
        var roles = user.Roles.Split(',');

        foreach (var role in roles)
            ci.AddClaim(new Claim(ClaimTypes.Role, role.Trim()));

        return ci;
    }

}
