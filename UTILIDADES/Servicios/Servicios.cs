using MimeKit;
using MailKit.Net.Smtp;
using UTILIDADES.Interfaces;
using MailKit.Security;
using MailKit;
using Microsoft.Extensions.Options;
using UTILIDADES.Modelos;

namespace UTILIDADES.Servicios
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly SmtpSettings _smtpSettings;

        public EmailService(IConfiguration configuration , IOptions<SmtpSettings> smtpSettings)
        {
            _configuration = configuration;
            _smtpSettings = smtpSettings.Value;
        }

        public async Task<string> SendEmailAsync(string to, string subject, string htmlBody, string? cc, string? bcc, Stream fileStream, string fileName)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_smtpSettings.NameSender, _smtpSettings.MailSender));
                message.To.Add(new MailboxAddress("", to));
                //Copias
                if (!string.IsNullOrEmpty(cc))
                {
                    foreach (var email in cc.Split(';'))
                    {
                        message.Cc.Add(new MailboxAddress("", email.Trim()));
                    }
                }

                //Copias Oculta
                if (!string.IsNullOrEmpty(bcc))
                {
                    foreach (var email in bcc.Split(';'))
                    {
                        message.Bcc.Add(new MailboxAddress("", email.Trim()));
                    }
                }

                message.Subject = subject;

                //message.Body = new TextPart("html") { Text = body };
                var textPart = new TextPart("html") { Text = htmlBody };

                MimePart attachmentPart = null;
                if (fileStream != null && !string.IsNullOrEmpty(fileName))
                {
                    attachmentPart = new MimePart()
                    {
                        Content = new MimeContent(fileStream),
                        ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                        ContentTransferEncoding = ContentEncoding.Base64,
                        FileName = fileName
                    };
                }

                var multipart = new Multipart("mixed") { textPart };
                if (attachmentPart != null)
                {
                    multipart.Add(attachmentPart);
                }

                message.Body = multipart;


                using (var client = new SmtpClient(new ProtocolLogger(Console.OpenStandardOutput())))
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    client.Connect(_smtpSettings.Server, _smtpSettings.Port, SecureSocketOptions.StartTls);

                    var userMail = _smtpSettings.Username;
                    var passwordMail = _smtpSettings.Password;
                    await client.AuthenticateAsync(userMail, passwordMail);

                    var serverResponse = await client.SendAsync(message);
                    await client.DisconnectAsync(true);

                   

                    return $"Correo enviado exitosamente. Respuesta del servidor: {serverResponse}";
                } 
            }
            catch (SmtpCommandException ex)
            {
                return $"Error SMTP: {ex.Message} (Código: {ex.StatusCode}) ";
            }
            catch (SmtpProtocolException ex)
            {
                return $"Error de protocolo SMTP: {ex.Message}";
            }
            catch (Exception ex)
            {
                return $"Error al enviar el correo: {ex.Message}";
            }




        }
    }
}
