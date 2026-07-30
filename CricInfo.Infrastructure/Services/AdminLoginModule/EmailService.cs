using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System.Net;

public class EmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendMail(
        string toEmail,
        string userName,
        string password)
    {
        try
        {
            ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls12;

            string from =
                _configuration["EmailSettings:From"]!;

            string host =
                _configuration["EmailSettings:Host"]!;

            int port =
                int.Parse(
                    _configuration["EmailSettings:Port"]!);

            string smtpUser =
                _configuration["EmailSettings:Username"]!;

            string smtpPassword =
                _configuration["EmailSettings:Password"]!;

            var email = new MimeMessage();

            email.From.Add(
                MailboxAddress.Parse(from));

            email.To.Add(
                MailboxAddress.Parse(toEmail));

            email.Subject =
                "CricInfo Admin Account Created";

            email.Body =
                new TextPart("plain")
                {
                    Text = $@"
Hello {userName},

Your CricInfo Admin account has been created.

-----------------------------------

Username : {userName}

Password : {password}

-----------------------------------

Login URL:
http://localhost:4200/admin

Please change your password after your first login.

Regards,
CricInfo Team
"
                };

            using var smtp = new SmtpClient();

            smtp.ServerCertificateValidationCallback =
                (s, c, h, e) => true;

            smtp.CheckCertificateRevocation = false;

            Console.WriteLine("Connecting...");

            await smtp.ConnectAsync(
                host,
                port,
                SecureSocketOptions.StartTls);

            Console.WriteLine(
                "Connected Successfully.");

            await smtp.AuthenticateAsync(
                smtpUser,
                smtpPassword);

            Console.WriteLine(
                "Authenticated Successfully.");

            await smtp.SendAsync(email);

            Console.WriteLine(
                "Mail Sent Successfully.");

            await smtp.DisconnectAsync(true);

            Console.WriteLine(
                "Disconnected.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                "================================");

            Console.WriteLine(
                "EMAIL ERROR");

            Console.WriteLine(
                ex.Message);

            if (ex.InnerException != null)
            {
                Console.WriteLine(
                    ex.InnerException.Message);
            }

            Console.WriteLine(
                "================================");

            throw;
        }
    }
}