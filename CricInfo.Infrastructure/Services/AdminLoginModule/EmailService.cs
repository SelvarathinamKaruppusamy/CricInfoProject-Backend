using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Net;

public class EmailService
{
    public async Task SendMail(
        string toEmail,
        string userName,
        string password)
    {
        try
        {
            ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls12;

            Console.WriteLine(
                $"DOT NET VERSION : {Environment.Version}");

            var email = new MimeMessage();

            email.From.Add(
                MailboxAddress.Parse(
                    "nihilritthik@gmail.com"));

            email.To.Add(
                MailboxAddress.Parse(
                    toEmail));

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

            using var smtp =
                new MailKit.Net.Smtp.SmtpClient();

            smtp.ServerCertificateValidationCallback =
                (s, c, h, e) => true;

            smtp.CheckCertificateRevocation = false;

            Console.WriteLine("Connecting...");

            await smtp.ConnectAsync(
                "smtp-relay.brevo.com",
                587,
                SecureSocketOptions.None);

            Console.WriteLine(
                "Connected Successfully.");

            await smtp.AuthenticateAsync(
                "b30784001@smtp-brevo.com",
                "xsmtpsib-5f25b586e31bf7412250462d0239947f9d02909831308e57a3dfe1a7342fb419-9UmKLa2azIPLArfV");

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