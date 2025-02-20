using Data.Helpers;
using MailKit.Net.Smtp;
using MimeKit;
using Service.Helpers;
using Service.Interfaces;

namespace Service.Implementations
{
    public class EmailService : IEmailService
    {
        #region Fields
        private readonly EmailSettings _emailSettings;
        #endregion

        #region Constructor
        public EmailService(EmailSettings emailSettings)
        {
            _emailSettings = emailSettings;
        }
        #endregion

        #region Handle Functions
        public async Task<bool> SendEmailAsync(EmailMessage email)
        {
            try
            {
                //sending the Message of passwordResetLink
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port, true);
                    client.Authenticate(_emailSettings.FromEmail, _emailSettings.Password);
                    var bodyBuilder = new BodyBuilder
                    {
                        TextBody = email.IsHtml ? null : email.Body,
                        HtmlBody = email.IsHtml ? email.Body : null
                    };

                    var message = new MimeMessage
                    {
                        Body = bodyBuilder.ToMessageBody()
                    };
                    message.From.Add(new MailboxAddress("Auth Project", _emailSettings.FromEmail));
                    // Add recipients
                    foreach (var recipient in email.To)
                        message.To.Add(MailboxAddress.Parse(recipient));
                    // Add CC
                    if (email.Cc is not null)
                    {
                        foreach (var cc in email.Cc)
                            message.Cc.Add(MailboxAddress.Parse(cc));
                    }

                    // Add BCC
                    if (email.Bcc is not null)
                    {
                        foreach (var bcc in email.Bcc)
                            message.Bcc.Add(MailboxAddress.Parse(bcc));
                    }
                    // Add attachments if any
                    if (email.Attachments is not null)
                    {
                        foreach (var attachment in email.Attachments)
                        {
                            using var stream = attachment.OpenReadStream();
                            bodyBuilder.Attachments.Add(attachment.FileName, stream);
                        }
                    }
                    message.Subject = email?.Subject;
                    // send email
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
                //end of sending email
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion
    }
}
