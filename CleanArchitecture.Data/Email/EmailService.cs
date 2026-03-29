using System;
using CleanArchitecture.Application.Contracts.Infrastructure;
using CleanArchitecture.Application.Models.Email;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace CleanArchitecture.Infrastructure.Email;

public class EmailService(EmailSettings emailSettings, ILogger<EmailService> logger) : IEmailService
{
    public EmailSettings _emailSettings { get; } = emailSettings;
    public ILogger<EmailService> _logger { get; } = logger;

    public async Task<bool> SendEmail(Application.Models.Email.Email email)
    {
        var client = new SendGridClient(_emailSettings.ApiKey);

        var from = new EmailAddress
        {
            Email = _emailSettings.FromAddress,
            Name = _emailSettings.FromName,
        };
        var to = new EmailAddress(email.To);
        var subject = email.Subject;
        var emailBody = email.Body;
        

        var sendGridMessage = MailHelper.CreateSingleEmail(from, to, subject, emailBody, emailBody);
        var response = await client.SendEmailAsync(sendGridMessage);

        if (response.StatusCode == System.Net.HttpStatusCode.Accepted || response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            return true;
        }
        else
        {
            _logger.LogError("Email couldn't be sent");
            return false;
        }
    }
}
