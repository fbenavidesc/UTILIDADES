namespace UTILIDADES.Interfaces
{
    public interface IEmailService
    {
        Task<string> SendEmailAsync(string to, string subject, string htmlBody, string? cc, string? bcc, Stream fileStream, string fileName);
    }
}
