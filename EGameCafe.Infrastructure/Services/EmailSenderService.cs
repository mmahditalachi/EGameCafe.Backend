using EGameCafe.Application.Common.Interfaces;
using EGameCafe.Application.Common.Models;
using Laboratory.Application.Common.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace Laboratory.Infrastructure.Services
{
    public class EmailSenderService : IEmailSender
    {
        private readonly EmailSettings _emailSettings;

        public EmailSenderService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task<Result> SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                var mimeMessage = new MimeMessage();

                mimeMessage.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.Sender));

                var address = new MailboxAddress(email);

                mimeMessage.To.Add(address);

                mimeMessage.Subject = subject;

                var builder = new BodyBuilder();

                builder.HtmlBody = "Hi this is mohi from dr talachi";

                using (var client = new SmtpClient())
                {
                    // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    //if (_env.IsDevelopment())
                    //{
                    // The third parameter is useSSL (true if the client should make an SSL-wrapped
                    // connection to the server; otherwise, false).
                    await client.ConnectAsync(_emailSettings.MailServer, _emailSettings.MailPort, true);
                    //}
                    //else
                    //{
                    //    await client.ConnectAsync(_emailSettings.MailServer);
                    //}

                    // Note: only needed if the SMTP server requires authentication
                    await client.AuthenticateAsync(_emailSettings.Sender, _emailSettings.Password);

                    await client.SendAsync(mimeMessage);

                    await client.DisconnectAsync(true);

                    return Result.Success();
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"email not sent to {email} errors : {ex.Message}");
            }
        }
    }
}
