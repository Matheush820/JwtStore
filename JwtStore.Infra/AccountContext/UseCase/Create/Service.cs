﻿using JwtStore.Core.Configurations;
using JwtStore.Core.Context.AccountContext.Entities;
using JwtStore.Core.Context.AccountContext.UseCases.Create.Contracts;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtStore.Infra.AccountContext.UseCase.Create;
public class Service : IService
{
    public async Task SendVerificationEmailAsync(User user, CancellationToken cancellationToken)
    {
        var client = new SendGridClient(Configuration.SendGrid.ApiKey);
        var from = new EmailAddress(Configuration.Email.DefaultFromEmail, Configuration.Email.DefaultFromName);
        var subject = "Verifique sua conta";
        var to = new EmailAddress(user.Email, user.Name);
        var content = $"Código {user.Email.Verification.Code}";

        var msg = MailHelper.CreateSingleEmail(from, to, subject,content, content);

        await client.SendEmailAsync(msg, cancellationToken);
    }
}
