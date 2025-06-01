using MailKit.Net.Imap;
using MailKit.Net.Smtp;
namespace Application.Interfaces
{
    public interface IEmailConnectionProvider
    {
        SmtpClient GetSmtpClient();
        ImapClient GetImapClient();
    }
}
