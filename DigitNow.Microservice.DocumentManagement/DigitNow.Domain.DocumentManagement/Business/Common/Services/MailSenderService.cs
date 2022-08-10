using DigitNow.Adapters.MS.Identity;
using DigitNow.Adapters.MS.Identity.Poco;
using DigitNow.Domain.Authentication.Contracts.Users.GetUsersByFilter;
using DigitNow.Domain.DocumentManagement.Business.Common.Notifications.Mail;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using Domain.Mail.Contracts.MailTemplates;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IMailSenderService
    {
        Task SendMail_SendBulkDocumentsTemplate(User headOfDepartmentUser, IList<long> documentIds, CancellationToken token);
        Task SendMail_DelegateDocumentToFunctionary(User currentUser, User delegatedUser, IList<long> documentIds, CancellationToken token);
        Task SendMail_DelegateDocumentToFunctionarySupervisor(User currentUser, User delegatedUser, IList<long> documentIds, CancellationToken token);
        Task SendMail_CreateIncomingDocument(User sender, long registrationId, DateTime date, CancellationToken token);
    }

    public class MailSenderService : IMailSenderService
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IMailSender _mailSender;
        private readonly IIdentityAdapterClient _identityAdapterClient;

        public MailSenderService(
            DocumentManagementDbContext dbContext, 
            IMailSender mailSender,
            IIdentityAdapterClient identityAdapterClient)
        {
            _dbContext = dbContext;
            _mailSender = mailSender;
            _identityAdapterClient = identityAdapterClient;
        }

        public async Task SendMail_SendBulkDocumentsTemplate(User headOfDepartmentUser, IList<long> documentIds, CancellationToken token)
        {
            var registryNumbers = await _dbContext.Documents
                .Where(x => documentIds.Contains(x.Id))
                .Select(x => x.RegistrationNumber)
                .ToArrayAsync(token);

            await _mailSender.SendMail(MailTemplateEnum.SendBulkDocumentsTemplate, headOfDepartmentUser.Email, new
            {
                RegistryNumbers = String.Join(',', registryNumbers)
            }, token);
        }

        public async Task SendMail_DelegateDocumentToFunctionary(User currentUser, User delegatedUser, IList<long> documentIds, CancellationToken token)
        {
            var registryNumbers = await _dbContext.Documents
                .Where(x => documentIds.Contains(x.Id))
                .Select(x => x.RegistrationNumber)
                .ToArrayAsync(token);
            
            await _mailSender.SendMail(MailTemplateEnum.DelegateDocumentToFunctionaryTemplate, delegatedUser.Email, new
            {
                UserName = $"{currentUser.LastName} {currentUser.FirstName}",
                RegistryNumbers = string.Join(',', registryNumbers)
            }, token);
        }

        public async Task SendMail_DelegateDocumentToFunctionarySupervisor(User currentUser, User delegatedUser, IList<long> documentIds, CancellationToken token)
        {
            var registryNumbers = await _dbContext.Documents
                .Where(x => documentIds.Contains(x.Id))
                .Select(x => x.RegistrationNumber)
                .ToArrayAsync(token);

            var userResponse = await _identityAdapterClient.GetUsersByRoleAndDepartment(RecipientType.HeadOfDepartment.Code, delegatedUser.Departments.FirstOrDefault(), token);

            var tasks = new List<Task>();
            foreach (var targetUser in userResponse.Users)
            {
                tasks.Add(_mailSender.SendMail(MailTemplateEnum.DelegateDocumentToFunctionarySupervisorTemplate, targetUser.Email, new
                {
                    UserName = $"{currentUser.LastName} {currentUser.FirstName}",
                    DelegateUserName = $"{delegatedUser.LastName} {delegatedUser.FirstName}",
                    DocumentJustification = string.Join(',', registryNumbers) 
                }, token));
            }

            await Task.WhenAll(tasks);
        }

        public async Task SendMail_CreateIncomingDocument(User sender, long registrationId, DateTime date, CancellationToken token)
        {
            var legalEntity = await _identityAdapterClient.GetCurrentLegalEntityAsync(token);

            await _mailSender.SendMail(MailTemplateEnum.AddRegistryTemplate, sender.Email,
                new
                {
                    Address = legalEntity?.Name,
                },
                new
                {
                    Address = legalEntity?.Name,
                    Name = $"{sender.FirstName} {sender.LastName}",
                    RegistryNumber = registrationId.ToString(),
                    Date = date.ToShortDateString(),
                }, token);
        }

        private class GetUsersByRoleAndDepartmentRequest : IGetUsersByRoleAndDepartmentRequest
        {
            public string RoleCode { get; set; }
            public long DepartmentId { get; set; }
        }
    }
}
