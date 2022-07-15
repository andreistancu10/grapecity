﻿using DigitNow.Adapters.MS.Identity.Poco;
using DigitNow.Domain.Authentication.Client;
using DigitNow.Domain.Authentication.Contracts.Users.GetUsersByFilter;
using DigitNow.Domain.DocumentManagement.Business.Common.Notifications.Mail;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using Domain.Mail.Contracts.MailTemplates;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IMailSenderService
    {
        Task SendMail_SendBulkDocumentsTemplate(User headOfDepartmentUser, IList<long> documentIds, CancellationToken token);
        Task SendMail_DelegateDocumentToFunctionary(User currentUser, User delegatedUser, IList<long> documentIds, CancellationToken token);
        Task SendMail_DelegateDocumentToFunctionarySupervisor(User currentUser, User delegatedUser, CancellationToken token);
    }

    public class MailSenderService : IMailSenderService
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IMailSender _mailSender;
        private readonly IAuthenticationClient _authenticationClient;

        public MailSenderService(
            DocumentManagementDbContext dbContext, 
            IMailSender mailSender,
            IAuthenticationClient authenticationClient)
        {
            _dbContext = dbContext;
            _mailSender = mailSender;
            _authenticationClient = authenticationClient;
        }

        public async Task SendMail_SendBulkDocumentsTemplate(User headOfDepartmentUser, IList<long> documentIds, CancellationToken token)
        {
            var registryNumbers = await _dbContext.Documents
                .Where(x => documentIds.Contains(x.Id))
                .Select(x => x.RegistrationNumber)
                .ToArrayAsync(token);

            await _mailSender.SendMail(MailTemplateEnum.SendBulkDocumentsTemplate, headOfDepartmentUser.Email, new
            {
                RegistryNumbers = registryNumbers
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
                UserName = currentUser.UserName,
                RegistryNumbers = registryNumbers
            }, token);
        }

        public async Task SendMail_DelegateDocumentToFunctionarySupervisor(User currentUser, User delegatedUser, CancellationToken token)
        {
            var request = new GetUsersByRoleAndDepartmentRequest
            {
                RoleCode = UserRole.HeadOfDepartment.Code,
                DepartmentId = delegatedUser.Departments.First() //TODO: Ask about this
            };

            var userResponse = await _authenticationClient.GetUsersByRoleAndDepartment(request, token);

            var tasks = new List<Task>();
            foreach (var targetUser in userResponse.Users)
            {
                tasks.Add(_mailSender.SendMail(MailTemplateEnum.DelegateDocumentToFunctionarySupervisorTemplate, targetUser.Email, new
                {
                    UserName = currentUser.UserName,
                    DelegateUserName = delegatedUser.UserName,
                    DocumentJustification = default(string) //TODO: Ask about this 
                }, token));
            }

            await Task.WhenAll(tasks);
        }

        private class GetUsersByRoleAndDepartmentRequest : IGetUsersByRoleAndDepartmentRequest
        {
            public string RoleCode { get; set; }
            public long DepartmentId { get; set; }
        }
    }
}
