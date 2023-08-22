using MimeKit;
using System.Threading.Tasks;
using MailKit.Net.Smtp;

namespace HOTELAPI1.Services
{
    public class EmailService
    {
        public async Task SendConfirmationEmailAsync(string email, string confirmationLink)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Alejandro Lara Polanco", "1106231@est.intec.edu.do"));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = "Confirma tu cuenta";

            var builder = new BodyBuilder();
            builder.HtmlBody = $"<a href='{confirmationLink}'><button>Confirmar Cuenta</button></a>";
            message.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp-relay.brevo.com", 587, false);
                await client.AuthenticateAsync("firemagic451@gmail.com", "atf4JBn1hLp2HNMc");
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }

}
