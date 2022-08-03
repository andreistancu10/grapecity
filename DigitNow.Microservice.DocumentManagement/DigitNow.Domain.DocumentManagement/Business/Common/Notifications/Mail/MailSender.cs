using Domain.Mail.Client;
using Domain.Mail.Contracts.FluentMailProviderType;
using Domain.Mail.Contracts.Mails;
using Domain.Mail.Contracts.MailTemplates;
using System.Text.Json;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Notifications.Mail
{
    public interface IMailSender
    {
        Task SendMail(MailTemplateEnum templateId, string mailTo, object mailBodyParameters, CancellationToken token);
        Task SendMail(MailTemplateEnum templateId, string mailTo, object mailSubjectParameters, object mailBodyParameters, CancellationToken token);
    }

    public class MailSender : IMailSender
    {
        private readonly IMailClient _mailClient;

        public MailSender(IMailClient mailClient)
        {
            _mailClient = mailClient;
        }

        public Task SendMail(MailTemplateEnum templateId, string mailTo, object mailBodyParameters, CancellationToken token) =>
            SendMail(templateId, mailTo, mailSubjectParameters: string.Empty, mailBodyParameters, token);

        public Task SendMail(MailTemplateEnum templateId, string mailTo, object mailSubjectParameters, object mailBodyParameters, CancellationToken token)
        {
            var mailEvent = new CreateMailEvent
            {
                FluentMailProviderTypeId = GetMailProvider(),
                SubjectParameters = JsonSerializer.Serialize(mailSubjectParameters),
                BodyParameters = JsonSerializer.Serialize(mailBodyParameters),
                MailTemplateId = templateId,
                Receiver = mailTo
            };

            return _mailClient.CreateMail(mailEvent, token);
        }

        private static FluentMailProviderTypeEnum GetMailProvider()
        {
            // TODO: Get from configuration
            return FluentMailProviderTypeEnum.FluentMailSendGrid;
        }
    }
}
