using DigitNow.Adapters.MS.Catalog;
using DigitNow.Domain.Authentication.Client;
using DigitNow.Domain.Authentication.Contracts.ContactDetails.GetLegalEntity;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.Notifications.Mail;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using Domain.Mail.Contracts.MailTemplates;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IMailSenderService
    {
        Task SendMail_OnSendBulkDocuments(UserModel headOfDepartmentUser, IList<long> documentIds, CancellationToken token);
        Task SendMail_OnDelegateDocumentToFunctionary(UserModel currentUser, UserModel delegatedUser, IList<long> documentIds, CancellationToken token);
        Task SendMail_OnDelegateDocumentToFunctionarySupervisor(UserModel currentUser, UserModel delegatedUser, IList<long> documentIds, CancellationToken token);
        Task SendMail_AfterIncomingDocumentCreated(UserModel sender, long registrationId, DateTime date, CancellationToken token);
        Task SendMail_OnIncomingDocDictributedToFunctionary(UserModel targetUser, Document document, CancellationToken token);
        Task SendMail_OnOpinionRequestedByAnotherDepartment(UserModel targetUser, long departmentId, Document document, CancellationToken token);
        Task SendMail_OnCompetenceDeclinedOnOpinionRequested(Document document, long senderDepartmentId, WorkflowHistoryLog historyLog, CancellationToken token);
        Task SendMail_OnApprovalRequestedByFunctionary(long recipientId, Document document, CancellationToken token);
        Task SendMail_OnApprovalRequestedByFunctionary(UserModel recipient, Document document, CancellationToken token);
        Task SendMail_OnDepartmentSupervisorApprovedDecision(Document document, long recipientId, CancellationToken token);
        Task SendMail_OnDepartmentSupervisorRejectedDecision(Document document, long recipientId, CancellationToken token);
        Task SendMail_OnReplySent(Document document, CancellationToken token);
        Task SendMail_OnCompetenceDeclined(Document document, int destinationDepartmentId, CancellationToken token);
        Task SendMail_OnMayorApprovedDecision(Document document, long recipientId, CancellationToken token);
        Task SendMail_OnMayorRejectedDecision(Document document, long recipientId, CancellationToken token);
        Task SendMail_OnSupervisorAssignedOpinionRequestToFunctionary(UserModel targetUser, Document document, CancellationToken token);
        Task SendMail_OnOpinionFunctionaryReplied(Document document, long supervisorId, long recipientId, CancellationToken token);
    }

    public class MailSenderService : IMailSenderService
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IMailSender _mailSender;
        private readonly ICatalogAdapterClient _catalogAdapterClient;
        private readonly IAuthenticationClient _authenticationClient;
        private readonly IIdentityService _identityService;
        private readonly IConfiguration _configuration;
        private readonly string _dateFormat;


        public MailSenderService(
            DocumentManagementDbContext dbContext, 
            IMailSender mailSender,
            ICatalogAdapterClient catalogAdapterClient,
            IAuthenticationClient authenticationClient,
            IIdentityService identityService,
            IConfiguration configuration)
        {
            _dbContext = dbContext;
            _mailSender = mailSender;
            _catalogAdapterClient = catalogAdapterClient;
            _authenticationClient = authenticationClient;
            _identityService = identityService;
            _configuration = configuration;

            _dateFormat = _configuration.GetValue<string>("Date:Format");
        }

        public async Task SendMail_OnSendBulkDocuments(UserModel headOfDepartmentUser, IList<long> documentIds, CancellationToken token)
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

        public async Task SendMail_OnDelegateDocumentToFunctionary(UserModel currentUser, UserModel delegatedUser, IList<long> documentIds, CancellationToken token)
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

        public async Task SendMail_OnDelegateDocumentToFunctionarySupervisor(UserModel currentUser, UserModel delegatedUser, IList<long> documentIds, CancellationToken token)
        {
            var headOfDepartmentUser = await _identityService.GetHeadOfDepartmentUserAsync(delegatedUser.Departments.First().Id, token);

            var registryNumbers = await _dbContext.Documents
                .Where(x => documentIds.Contains(x.Id))
                .Select(x => x.RegistrationNumber)
                .ToArrayAsync(token);

            await _mailSender.SendMail(MailTemplateEnum.DelegateDocumentToFunctionarySupervisorTemplate, headOfDepartmentUser.Email, new
            {
                UserName = $"{currentUser.LastName} {currentUser.FirstName}",
                DelegateUserName = $"{delegatedUser.LastName} {delegatedUser.FirstName}",
                DocumentJustification = string.Join(',', registryNumbers) 
            }, token);            
        }

        public async Task SendMail_AfterIncomingDocumentCreated(UserModel sender, long registrationId, DateTime date, CancellationToken token)
        {
            var legalEntityResponse = await _authenticationClient.ContactDetails.GetLegalEntityAsync(new GetLegalEntityRequest(), token);

            var legalEntity = legalEntityResponse.LegalEntity;

            await _mailSender.SendMail(MailTemplateEnum.AddRegistryTemplate, sender.Email,
                new
                {
                    Address = legalEntity.Name,
                },
                new
                {
                    Address = legalEntity.Name,
                    Name = $"{sender.FirstName} {sender.LastName}",
                    RegistryNumber = registrationId.ToString(),
                    Date = date.ToString(_dateFormat)
                }, token);
        }

        public Task SendMail_OnIncomingDocDictributedToFunctionary(UserModel targetUser, Document document , CancellationToken token)
        {
            return SendMail_WithRegistrationAndDateAsync(targetUser.Email, document, MailTemplateEnum.DistributionOfIncomingDocumentToFunctionaryTemplate, token);
        }

        public async Task SendMail_OnOpinionRequestedByAnotherDepartment(UserModel targetUser, long departmentId, Document document, CancellationToken token)
        {
            var department = await _catalogAdapterClient.GetDepartmentByIdAsync(departmentId, token);

            await _mailSender.SendMail(MailTemplateEnum.OpinionRequestedByAnotherDepartmentTemplate, targetUser.Email,
                new
                {
                    Structure = department.Name
                },
                new
                {
                    Structure = department.Name,
                    RegistryNumber = document.RegistrationNumber,
                    Date = document.RegistrationDate.ToString("dd/MM/YYYY")
                }, token);
        }

        public async Task SendMail_OnCompetenceDeclinedOnOpinionRequested(Document document, long senderDepartmentId, WorkflowHistoryLog historyLog, CancellationToken token)
        {

            var whoAskedForOpinion = historyLog.CreatedBy;
            var senderDepartment = await _catalogAdapterClient.GetDepartmentByIdAsync(senderDepartmentId, token);

            var recipient = await _identityService.GetUserByIdAsync(whoAskedForOpinion, token);

            if(recipient == null || senderDepartment == null)
            {
                throw new ArgumentException("Recipient and sender cannot be found", nameof(document));
            }

            await _mailSender.SendMail(MailTemplateEnum.DeclineCompetenceOpinionTemplate, recipient.Email,
               new
               {
                   Structure = senderDepartment.Name
               },
               new
               {
                   Structure = senderDepartment.Name,
                   RegistryNumber = document.RegistrationNumber,
                   Date = document.RegistrationDate.ToString(_dateFormat)
               }, token);
        }

        public async Task SendMail_OnApprovalRequestedByFunctionary(long recipientId, Document document, CancellationToken token)
        {
            var recipient = await _identityService.GetUserByIdAsync(recipientId, token);

            await SendMail_WithRegistrationAndDateAsync(recipient.Email, document, MailTemplateEnum.ApprovalRequestByFunctionaryTemplate, token);
        }

        public Task SendMail_OnApprovalRequestedByFunctionary(UserModel recipient, Document document, CancellationToken token)
        {
            return SendMail_WithRegistrationAndDateAsync(recipient.Email, document, MailTemplateEnum.ApprovalRequestByFunctionaryTemplate, token);
        }

        public async Task SendMail_OnDepartmentSupervisorApprovedDecision(Document document, long recipientId, CancellationToken token)
        {
            
            var recipient = await _identityService.GetUserByIdAsync(recipientId, token);
            if (recipient == null)
            {
                throw new ArgumentException("Recipient cannot be found", nameof(document));
            }
            await SendMail_WithRegistrationAndDateAsync(recipient.Email, document, MailTemplateEnum.DepartmentSupervisorApprovalDecisionTemplate, token);
            
        }

        public async Task SendMail_OnDepartmentSupervisorRejectedDecision(Document document, long recipientId, CancellationToken token)
        {

            var recipient = await _identityService.GetUserByIdAsync(recipientId, token);
            if (recipient == null)
            {
                throw new ArgumentException("Recipient cannot be found", nameof(document));
            }
            await SendMail_WithRegistrationAndDateAsync(recipient.Email, document, MailTemplateEnum.DepartmentSupervisorRejectionDecisionTemplate, token);
            
        }

        public async Task SendMail_OnReplySent(Document document, CancellationToken token)
        {
            var department = await _catalogAdapterClient.GetDepartmentByCodeAsync(UserDepartment.Registry.Code, token);
            if (department == null) return;

            var departmentUsers = await _identityService.GetUsersWithinDepartmentAsync(department.Id, token);

            var tasks = new List<Task>();
            foreach (var departmentUser in departmentUsers)
            {
                tasks.Add(SendMail_WithRegistrationAndDateAsync(departmentUser.Email, document, MailTemplateEnum.SendingReplyTemplate, token));
            }
            await Task.WhenAll(tasks);
        }

        private Task SendMail_WithRegistrationAndDateAsync(string recipient, Document document, MailTemplateEnum mailTemplate, CancellationToken token)
        {
            return _mailSender.SendMail(mailTemplate, recipient,
            new
            {
                RegistryNumber = document.RegistrationNumber,
                Date = document.RegistrationDate.ToString(_dateFormat)
            },
            new
            {
                RegistryNumber = document.RegistrationNumber,
                Date = document.RegistrationDate.ToString(_dateFormat)
            }, token);
        }

        public async Task SendMail_OnCompetenceDeclined(Document document, int destinationDepartmentId, CancellationToken token)
        {

            var userWhoRegisteredTheDocument = await _identityService.GetUserByIdAsync(document.CreatedBy, token);
            if (userWhoRegisteredTheDocument != null)
            {
                var department = await _catalogAdapterClient.GetDepartmentByIdAsync(destinationDepartmentId, token);

                if (department == null)
                {
                    throw new ArgumentException("Department cannot be found", nameof(document));
                }

                await _mailSender.SendMail(MailTemplateEnum.DeclineCompetenceTemplate, userWhoRegisteredTheDocument.Email,
                new
                {
                    Structure = department.Name
                },
                new
                {
                    Structure = department.Name,
                    RegistryNumber = document.RegistrationNumber,
                    Date = document.RegistrationDate.ToString(_dateFormat)
                }, token);
            }
        }

        public async Task SendMail_OnMayorApprovedDecision(Document document, long recipientId, CancellationToken token)
        {
            var recipient = await _identityService.GetUserByIdAsync(recipientId, token);
            if (recipient == null)
            {
                throw new ArgumentException("Recipient cannot be found", nameof(document));
            }

            await SendMail_WithRegistrationAndDateAsync(recipient.Email, document, MailTemplateEnum.MayorApprovalDecisionTemplate, token);
            
        }

        public async Task SendMail_OnMayorRejectedDecision(Document document, long recipientId, CancellationToken token)
        {
            var recipient = await _identityService.GetUserByIdAsync(recipientId, token);

            await SendMail_WithRegistrationAndDateAsync(recipient.Email, document, MailTemplateEnum.MayorRejectionDecisionTemplate, token);
        }

        public Task SendMail_OnSupervisorAssignedOpinionRequestToFunctionary(UserModel targetUser, Document document, CancellationToken token)
        {
            return SendMail_WithRegistrationAndDateAsync(targetUser.Email, document, MailTemplateEnum.OpinionSupervisorToFunctionaryTemplate, token);
        }

        public async Task SendMail_OnOpinionFunctionaryReplied(Document document, long supervisorId, long recipientId,  CancellationToken token)
        {
            var supervisorWhoSentForOpinion = await _identityService.GetUserByIdAsync(supervisorId, token);
            if(supervisorWhoSentForOpinion == null)
            {
                throw new ArgumentException("Supervisor who asked for opinion cannot be found", nameof(document));
            }

            var department = await _catalogAdapterClient.GetDepartmentByIdAsync(supervisorWhoSentForOpinion.Departments.First().Id, token);
            var recipient = await _identityService.GetUserByIdAsync(recipientId, token);
            if(department == null || recipient == null)
            {
                throw new ArgumentException("Department and recipient cannot be found", nameof(document));
            }
            
            await _mailSender.SendMail(MailTemplateEnum.OpinionFunctionaryReplyTemplate, recipient.Email,
                new
                {
                    RegistryNumber = document.RegistrationNumber,
                    Date = document.RegistrationDate.ToString(_dateFormat)
                },
                new
                {
                    Structure = department.Name,
                    RegistryNumber = document.RegistrationNumber,
                    Date = document.RegistrationDate.ToString(_dateFormat)

                }, token);
        }
    }
}
