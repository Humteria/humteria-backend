namespace Humteria.Data.DTOs.Mail;

internal class MailToDTO
{
    public string Recipient { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Html { get; set; } = string.Empty;
}
