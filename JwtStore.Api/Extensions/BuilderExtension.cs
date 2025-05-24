using JwtStore.Core.Configurations;
using JwtStore.Infra.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace JwtStore.Api.Extensions;

public static class BuilderExtension
{
    public static void AddConfiguration(this WebApplicationBuilder builder)
    {
        Configuration.Database.ConnectionString =
            builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;

        Configuration.Secrets.ApiKey =
            builder.Configuration.GetSection("Secrets").GetValue<string>("ApiKey") ?? string.Empty;

        Configuration.Secrets.ApiKey =
           builder.Configuration.GetSection("Secrets").GetValue<string>("JwtPrivateKey") ?? string.Empty;
        Configuration.Secrets.ApiKey =
           builder.Configuration.GetSection("Secrets").GetValue<string>("PasswordSaltKey") ?? string.Empty;

        Configuration.Secrets.ApiKey =
       builder.Configuration.GetSection("SendGrid").GetValue<string>("PasswordSaltKey") ?? string.Empty;

        Configuration.Email.DefaultFromName =
       builder.Configuration.GetSection("Email").GetValue<string>("PasswordSaltKey") ?? string.Empty;

        Configuration.Email.DefaultFromEmail =
       builder.Configuration.GetSection("Email").GetValue<string>("PasswordSaltKey") ?? string.Empty;
    }

    public static void AddDataBase(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer(Configuration.Database.ConnectionString, b => b.MigrationsAssembly("JwtStore.Api")));
    }

    public static void AddJwtAuthentication(this WebApplicationBuilder builder)
    {
        var secretKey = builder.Configuration["Jwt:Secret"];

        builder.Services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });

        builder.Services.AddAuthorization();
    }

    public static void AddMediator(this WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(Configuration).Assembly));
    }
}
