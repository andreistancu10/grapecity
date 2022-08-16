using DigitNow.Adapters.MS.Catalog;
using DigitNow.Adapters.MS.Identity;
using DigitNow.Adapters.MS.Identity.Poco;
using DigitNow.Domain.Authentication.Client;
using DigitNow.Domain.Authentication.Contracts.Users.GetUsersByFilter;
using DigitNow.Domain.DocumentManagement.Business.Common.Notifications.Mail;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
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
        Task SendMail_DistributeIncomingDocToFunctionary(User targetUser, Document document, CancellationToken token);
        Task SendMail_OpinionRequestedByAnotherDepartment(User targetUser, long departmentId, Document document, CancellationToken token);
        Task SendMail_DeclineCompetenceOpinion(Document document, WorkflowHistoryLog historyLog, CancellationToken token);
        Task SendMail_ApprovalRequestedByFunctionary(long recipientId, Document document, CancellationToken token);
        Task SendMail_ApprovalRequestedByFunctionary(User recipient, Document document, CancellationToken token);
        Task SentMail_DepartmentSupervisorApprovalDecision(Document document, CancellationToken token);
        Task SentMail_DepartmentSupervisorRejectionDecision(Document document, CancellationToken token);
        Task SendMail_SendingReply(Document document, CancellationToken token);
        Task SendMail_DeclineCompetence(Document document, WorkflowHistoryLog historyLog, CancellationToken token);
        Task SendMail_MayorApprovalDecision(Document document, WorkflowHistoryLog historyLog, CancellationToken token);
        Task SendMail_MayorRejectionDecision(Document document, WorkflowHistoryLog historyLog, CancellationToken token);
        Task SendMail_OpinionSupervisorToFunctionary(User targetUser, Document document, CancellationToken token);
        Task SendMail_OpinionFunctionaryReply(Document document, CancellationToken token);
    }

    public class MailSenderService : IMailSenderService
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IMailSender _mailSender;
        private readonly IIdentityAdapterClient _identityAdapterClient;
        private readonly ICatalogAdapterClient _catalogAdapterClient;

        public MailSenderService(
            DocumentManagementDbContext dbContext, 
            IMailSender mailSender,
            IIdentityAdapterClient identityAdapterClient,
            ICatalogAdapterClient catalogAdapterClient)
        {
            _dbContext = dbContext;
            _mailSender = mailSender;
            _identityAdapterClient = identityAdapterClient;
            _catalogAdapterClient = catalogAdapterClient;
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

        public async Task SendMail_DistributeIncomingDocToFunctionary(User targetUser, Document document , CancellationToken token)
        {
            await SendMail_WithRegistrationAndDateAsync(targetUser.Email, document, MailTemplateEnum.DistributionOfIncomingDocumentToFunctionaryTemplate, token);
        }

        public async Task SendMail_OpinionRequestedByAnotherDepartment(User targetUser, long departmentId, Document document, CancellationToken token)
        {
            var department = await _catalogAdapterClient.GetDepartmentByIdAsync(document.DestinationDepartmentId, token);

            await _mailSender.SendMail(MailTemplateEnum.OpinionRequestedByAnotherDepartmentTemplate, targetUser.Email,
                new
                {
                    Structure = department.Name
                },
                new
                {
                    Structure = department.Name,
                    RegistryNumber = document.RegistrationNumber,
                    Date = document.RegistrationDate.ToShortDateString()
                }, token);
        }

        public async Task SendMail_DeclineCompetenceOpinion(Document document, WorkflowHistoryLog historyLog, CancellationToken token)
        {
            var recipientUserHistoryLog = await _identityAdapterClient.GetUserByIdAsync(historyLog.RecipientId, token);
            if(recipientUserHistoryLog != null && recipientUserHistoryLog.Departments.Any())
            {
                var department = await _catalogAdapterClient.GetDepartmentByIdAsync(recipientUserHistoryLog.Departments.FirstOrDefault(), token);
                var userWhoAdskedForOpinion = historyLog.CreatedBy;
                var recipient = await _identityAdapterClient.GetUserByIdAsync(userWhoAdskedForOpinion, token);

                await _mailSender.SendMail(MailTemplateEnum.DeclineCompetenceOpinionTemplate, recipient.Email,
                   new
                   {
                       Structure = department.Name
                   },
                   new
                   {
                       Structure = department.Name,
                       RegistryNumber = document.RegistrationNumber,
                       Date = document.RegistrationDate.ToShortDateString()
                   }, token);
            }
        }

        public async Task SendMail_ApprovalRequestedByFunctionary(long recipientId, Document document, CancellationToken token)
        {
            var recipient = await _identityAdapterClient.GetUserByIdAsync(recipientId, token);

            await SendMail_WithRegistrationAndDateAsync(recipient.Email, document, MailTemplateEnum.ApprovalRequestByFunctionaryTemplate, token);
        }

        public async Task SendMail_ApprovalRequestedByFunctionary(User recipient, Document document, CancellationToken token)
        {
            await SendMail_WithRegistrationAndDateAsync(recipient.Email, document, MailTemplateEnum.ApprovalRequestByFunctionaryTemplate, token);
        }

        public async Task SentMail_DepartmentSupervisorApprovalDecision(Document document, CancellationToken token)
        {
            var previousResponsible = document.WorkflowHistories.OrderByDescending(x => x.CreatedAt).Skip(1).FirstOrDefault();
            if (previousResponsible != null)
            {
                var recipient = await _identityAdapterClient.GetUserByIdAsync(previousResponsible.RecipientId, token);
                await SendMail_WithRegistrationAndDateAsync(recipient.Email, document, MailTemplateEnum.DepartmentSupervisorApprovalDecisionTemplate, token);
            }
        }

        public async Task SentMail_DepartmentSupervisorRejectionDecision(Document document, CancellationToken token)
        {
            var previousResponsible = document.WorkflowHistories.OrderByDescending(x => x.CreatedAt).Skip(1).FirstOrDefault();
            if (previousResponsible != null)
            {
                var recipient = await _identityAdapterClient.GetUserByIdAsync(previousResponsible.RecipientId, token);
                await SendMail_WithRegistrationAndDateAsync(recipient.Email, document, MailTemplateEnum.DepartmentSupervisorRejectionDecisionTemplate, token);
            }
        }

        public async Task SendMail_SendingReply(Document document, CancellationToken token)
        {
            var department = await _catalogAdapterClient.GetDepartmentByCodeAsync("registratura", token);
            if (department != null)
            {
                //use method from authentiocationClient  await _authenticationClient.GetUsersByDepartment(department.Id, token);
                var usersFromRegistry = await _identityAdapterClient.GetUsersByDepartment(department.Id, token);

                var tasks = new List<Task>();
                foreach (var targetUser in usersFromRegistry.Users)
                {
                    tasks.Add(SendMail_WithRegistrationAndDateAsync(targetUser.Email, document, MailTemplateEnum.SendingReplyTemplate, token));
                }
                await Task.WhenAll(tasks);
            }
        }

        private async Task SendMail_WithRegistrationAndDateAsync(string recipient, Document document, MailTemplateEnum mailTemplate, CancellationToken token)
        {
            await _mailSender.SendMail(mailTemplate, recipient,
            new
            {
                RegistryNumber = document.RegistrationNumber,
                Date = document.RegistrationDate.ToShortDateString()
            },
            new
            {
                RegistryNumber = document.RegistrationNumber,
                Date = document.RegistrationDate.ToShortDateString()
            }, token);
        }

        public async Task SendMail_DeclineCompetence(Document document, WorkflowHistoryLog historyLog, CancellationToken token)
        {
            var userWhoRegisteredId = document.CreatedBy;

            var userWhoRegistered = await _identityAdapterClient.GetUserByIdAsync(userWhoRegisteredId, token);
            if (userWhoRegistered != null && userWhoRegistered.Departments.Any())
            {
                var department = await _catalogAdapterClient.GetDepartmentByIdAsync(userWhoRegistered.Departments.FirstOrDefault(), token);
                await _mailSender.SendMail(MailTemplateEnum.DeclineCompetenceTemplate, userWhoRegistered.Email,
                new
                {
                    Structure = department.Name
                },
                new
                {
                    Structure = department.Name,
                    RegistryNumber = document.RegistrationNumber,
                    Date = document.RegistrationDate.ToShortDateString()
                }, token);
            }
        }

        public async Task SendMail_MayorApprovalDecision(Document document, WorkflowHistoryLog historyLog, CancellationToken token)
        {
            var recipient = await _identityAdapterClient.GetUserByIdAsync(historyLog.RecipientId, token);

            await SendMail_WithRegistrationAndDateAsync(recipient.Email, document, MailTemplateEnum.MayorApprovalDecisionTemplate, token);
        }

        public async Task SendMail_MayorRejectionDecision(Document document, WorkflowHistoryLog historyLog, CancellationToken token)
        {
            var recipient = await _identityAdapterClient.GetUserByIdAsync(historyLog.RecipientId, token);

            await SendMail_WithRegistrationAndDateAsync(recipient.Email, document, MailTemplateEnum.MayorRejectionDecisionTemplate, token);
        }

        public async Task SendMail_OpinionSupervisorToFunctionary(User targetUser, Document document, CancellationToken token)
        {
            await SendMail_WithRegistrationAndDateAsync(targetUser.Email, document, MailTemplateEnum.OpinionSupervisorToFunctionaryTemplate, token);
        }

        public async Task SendMail_OpinionFunctionaryReply(Document document, CancellationToken token)
        {
            var historyLog = document.WorkflowHistories.FirstOrDefault(x => x.DocumentStatus == DocumentStatus.OpinionRequestedAllocated);

            var supervisorWhoSentForOpinion = await _identityAdapterClient.GetUserByIdAsync(historyLog.CreatedBy, token);
            var department = await _catalogAdapterClient.GetDepartmentByIdAsync(supervisorWhoSentForOpinion.Departments.FirstOrDefault(), token);
            await _mailSender.SendMail(MailTemplateEnum.OpinionFunctionaryReplyTemplate, supervisorWhoSentForOpinion.Email,
            new
            {
                RegistryNumber = document.RegistrationNumber,
                Date = document.RegistrationDate.ToShortDateString()
            },
            new
            {
                Structure = department.Name,
                RegistryNumber = document.RegistrationNumber,
                Date = document.RegistrationDate.ToShortDateString(),
               
            }, token);
        }

        private class GetUsersByRoleAndDepartmentRequest : IGetUsersByRoleAndDepartmentRequest
        {
            public string RoleCode { get; set; }
            public long DepartmentId { get; set; }
        }
    }
}
