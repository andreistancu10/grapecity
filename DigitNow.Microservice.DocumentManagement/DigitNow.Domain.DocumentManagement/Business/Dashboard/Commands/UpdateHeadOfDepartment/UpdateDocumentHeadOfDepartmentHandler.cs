﻿using DigitNow.Adapters.MS.Identity;
using DigitNow.Adapters.MS.Identity.Poco;
using DigitNow.Domain.DocumentManagement.Business.Common.Factories;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Dashboard.Commands.Update
{
    public class UpdateDocumentHeadOfDepartmentHandler : ICommandHandler<UpdateDocumentHeadOfDepartmentCommand, ResultObject>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IIdentityAdapterClient _identityAdapterClient;
        private User _headOfDepartment;

        public UpdateDocumentHeadOfDepartmentHandler(DocumentManagementDbContext dbContext, IIdentityAdapterClient identityAdapterClient)
        {
            _dbContext = dbContext;
            _identityAdapterClient = identityAdapterClient;
        }
        public async Task<ResultObject> Handle(UpdateDocumentHeadOfDepartmentCommand request, CancellationToken cancellationToken)
        {
            var response = await _identityAdapterClient.GetUsersAsync(cancellationToken);
            var departmentUsers = response.Users.Where(x => x.Departments.Contains(request.DepartmentId));
            _headOfDepartment = departmentUsers.FirstOrDefault(x => x.Roles.Contains(UserRole.HeadOfDepartment.Code));

            if (_headOfDepartment == null)
                return ResultObject.Error(new ErrorMessage
                {
                    Message = $"No responsible for department with id {request.DepartmentId} was found.",
                    TranslationCode = "catalog.headOfdepartment.backend.update.validation.entityNotFound",
                    Parameters = new object[] { request.DepartmentId }
                });

            await UpdateDocuments(request);
            await _dbContext.SaveChangesAsync();

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
                    default:
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
