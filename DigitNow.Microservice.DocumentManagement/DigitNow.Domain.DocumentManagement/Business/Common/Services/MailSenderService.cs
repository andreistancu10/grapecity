﻿using DigitNow.Adapters.MS.Catalog;
using DigitNow.Adapters.MS.Identity;
using DigitNow.Adapters.MS.Identity.Poco;
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
        Task SendMail_DeclineCompetenceOpinion(User targetUser, long departmentId, Document document, CancellationToken token);
        Task SendMail_ApprovalRequestedByFunctionary(long recipientId, Document document, CancellationToken token);
        Task SendMail_ApprovalRequestedByFunctionary(User recipient, Document document, CancellationToken token);
        Task SentMail_DepartmentSupervisorApprovalDecision(Document document, CancellationToken token);
        Task SentMail_DepartmentSupervisorRejectionDecision(Document document, CancellationToken token);
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

        public async Task SendMail_DistributeIncomingDocToFunctionary(User recipient, Document document , CancellationToken token)
        {
            await SendMail_WithRegistrationAndDateAsync(recipient, document, MailTemplateEnum.DistributionOfIncomingDocumentToFunctionaryTemplate, token);
        }

        public async Task SendMail_OpinionRequestedByAnotherDepartment(User targetUser, long departmentId, Document document, CancellationToken token)
        {
            var department = await _catalogAdapterClient.GetDepartmentByIdAsync(document.DestinationDepartmentId, token);

            await _mailSender.SendMail(MailTemplateEnum.OpinionRequestedByAnotherDepartmentTemplate, targetUser.Email,
                new
                {
                    Structure = department
                },
                new
                {
                    Structure = department,
                    RegistryNumber = document.RegistrationNumber,
                    Date = document.RegistrationDate.ToShortDateString()
                }, token);
        }

        public async Task SendMail_DeclineCompetenceOpinion(User targetUser, long departmentId, Document document, CancellationToken token)
        {
            var department = await _catalogAdapterClient.GetDepartmentByIdAsync(document.DestinationDepartmentId, token);

            await _mailSender.SendMail(MailTemplateEnum.DeclineCompetenceOpinionTemplate, targetUser.Email,
               new
               {
                   Structure = department
               },
               new
               {
                   Structure = department,
                   RegistryNumber = document.RegistrationNumber,
                   Date = document.RegistrationDate.ToShortDateString()
               }, token);
        }

        public async Task SendMail_ApprovalRequestedByFunctionary(long recipientId, Document document, CancellationToken token)
        {
            var recipient = await _identityAdapterClient.GetUserByIdAsync(recipientId, token);

            await SendMail_WithRegistrationAndDateAsync(recipient, document, MailTemplateEnum.ApprovalRequestByFunctionaryTemplate, token);
        }

        public async Task SendMail_ApprovalRequestedByFunctionary(User recipient, Document document, CancellationToken token)
        {
            await SendMail_WithRegistrationAndDateAsync(recipient, document, MailTemplateEnum.ApprovalRequestByFunctionaryTemplate, token);
        }

        public async Task SentMail_DepartmentSupervisorApprovalDecision(Document document, CancellationToken token)
        {
            var previousResponsible = document.WorkflowHistories.OrderByDescending(x => x.CreatedAt).Skip(1).FirstOrDefault();
            if (previousResponsible != null)
            {
                var recipient = await _identityAdapterClient.GetUserByIdAsync(previousResponsible.CreatedBy, token);
                await SendMail_WithRegistrationAndDateAsync(recipient, document, MailTemplateEnum.DepartmentSupervisorApprovalDecisionTemplate, token);
            }
        }

        public async Task SentMail_DepartmentSupervisorRejectionDecision(Document document, CancellationToken token)
        {
            var previousResponsible = document.WorkflowHistories.OrderByDescending(x => x.CreatedAt).Skip(1).FirstOrDefault();
            if (previousResponsible != null)
            {
                var recipient = await _identityAdapterClient.GetUserByIdAsync(previousResponsible.CreatedBy, token);
                await SendMail_WithRegistrationAndDateAsync(recipient, document, MailTemplateEnum.DepartmentSupervisorRejectionDecisionTemplate, token);
            }
        }

        private async Task SendMail_WithRegistrationAndDateAsync(User recipient, Document document, MailTemplateEnum mailTemplate, CancellationToken token)
        {
            await _mailSender.SendMail(mailTemplate, recipient.Email,
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

        private class GetUsersByRoleAndDepartmentRequest : IGetUsersByRoleAndDepartmentRequest
        {
            public string RoleCode { get; set; }
            public long DepartmentId { get; set; }
        }
    }
}
