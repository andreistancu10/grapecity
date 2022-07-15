using AutoMapper;
using DigitNow.Adapters.MS.Identity;
using DigitNow.Adapters.MS.Identity.Poco;
using DigitNow.Domain.Authentication.Client;
using DigitNow.Domain.Authentication.Contracts.Users.GetUsersByFilter;
using DigitNow.Domain.DocumentManagement.Business.Common.Factories;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Dashboard.Commands.Update
{
    public class UpdateDocumentHeadOfDepartmentHandler : ICommandHandler<UpdateDocumentHeadOfDepartmentCommand, ResultObject>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IMailSenderService _mailSenderService;
        private readonly IAuthenticationClientAdapter _authenticationClientAdapter;
        private User _headOfDepartment;

        public UpdateDocumentHeadOfDepartmentHandler(
            DocumentManagementDbContext dbContext, 
            IMailSenderService mailSenderService,
            IAuthenticationClientAdapter authenticationClientAdapter)
        {
            _dbContext = dbContext;
            _mailSenderService = mailSenderService;
            _authenticationClientAdapter = authenticationClientAdapter;
        }
        public async Task<ResultObject> Handle(UpdateDocumentHeadOfDepartmentCommand request, CancellationToken cancellationToken)
        {
            var headOfDepartmentUsers = await _authenticationClientAdapter
                .GetUsersByRoleAndDepartmentAsync(UserRole.HeadOfDepartment.Code, request.DepartmentId, cancellationToken);            

            _headOfDepartment = headOfDepartmentUsers.First();

            if (_headOfDepartment == null)
                return ResultObject.Error(new ErrorMessage
                {
                    Message = $"No responsible for department with id {request.DepartmentId} was found.",
                    TranslationCode = "catalog.headOfdepartment.backend.update.validation.entityNotFound",
                    Parameters = new object[] { request.DepartmentId }
                });

            await UpdateDocuments(request);
            await _dbContext.SaveChangesAsync();

            var documentIds = request.DocumentInfo.Select(x => x.DocumentId).ToList();
            await _mailSenderService.SendMail_SendBulkDocumentsTemplate(_headOfDepartment, documentIds, cancellationToken);

            return new ResultObject(ResultStatusCode.Ok);
        }

        private async Task UpdateDocuments(UpdateDocumentHeadOfDepartmentCommand request)
        {
            foreach (var docInfo in request.DocumentInfo)
            {
                var document = await _dbContext.Documents.FirstAsync(x => x.Id == docInfo.DocumentId);

                switch (document.DocumentType)
                {
                    case DocumentType.Incoming:
                        await UpdateHeadOfDepartment<IncomingDocument>(document);
                        break;
                    case DocumentType.Internal:
                        await UpdateHeadOfDepartment<InternalDocument>(document);
                        break;
                    case DocumentType.Outgoing:
                        await UpdateHeadOfDepartment<OutgoingDocument>(document);
                        break;
                }
            }
        }

        private async Task UpdateHeadOfDepartment<T>(Document document) where T : VirtualDocument
        {
            var virtualDocument = await _dbContext.Set<T>().AsQueryable()
                .FirstOrDefaultAsync(x => x.DocumentId == document.Id);

            document.Status = DocumentStatus.InWorkUnallocated;
            document.RecipientId = (int)_headOfDepartment.Id;
            virtualDocument.WorkflowHistory.Add(WorkflowHistoryFactory.Create(UserRole.HeadOfDepartment, _headOfDepartment, DocumentStatus.InWorkUnallocated));
        }
    }
}
