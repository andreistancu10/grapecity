using DigitNow.Adapters.MS.Identity;
using DigitNow.Adapters.MS.Identity.Poco;
using DigitNow.Domain.DocumentManagement.Business.Common.Factories;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
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
            _headOfDepartment = departmentUsers.FirstOrDefault(x => x.Roles.Contains((long)UserRole.HeadOfDepartment));

            if (_headOfDepartment == null)
                return ResultObject.Error(new ErrorMessage
                {
                    Message = $"No responsible for department with id {request.DepartmentId} was found.",
                    TranslationCode = "catalog.headOfdepartment.backend.update.validation.entityNotFound",
                    Parameters = new object[] { request.DepartmentId }
                });

            await UpdateDocumentHeadOfDepartment(request);
            await _dbContext.SaveChangesAsync();

            return new ResultObject(ResultStatusCode.Ok);
        }

        private async Task UpdateDocumentHeadOfDepartment(UpdateDocumentHeadOfDepartmentCommand request)
        {
            await UpdateHeadOfDepartmentForIncomingDocuments(request);
            await UpdateHeadOfDepartmentForInternalDocuments(request);
            await UpdateHeadOfDepartmentForOutgoingDocuments(request);
        }

        private async Task UpdateHeadOfDepartmentForOutgoingDocuments(UpdateDocumentHeadOfDepartmentCommand request)
        {
            var outgoingDocIds = request.DocumentInfo
                                        .Where(doc => doc.DocType == (int)DocumentType.Outgoing)
                                        .Select(doc => doc.RegistrationNumber)
                                        .ToList();

            foreach (var registrationNo in outgoingDocIds)
            {
                var outgoingDoc = await _dbContext.OutgoingDocuments
                    .Include(x => x.Document)
                    .FirstOrDefaultAsync(x => x.Document.RegistrationNumber == registrationNo);

                if (outgoingDoc != null)
                {
                    outgoingDoc.Document.Status = DocumentStatus.InWorkUnallocated;
                    outgoingDoc.RecipientId = (int)_headOfDepartment.Id;
                    outgoingDoc.WorkflowHistory.Add(WorkflowHistoryFactory.Create(outgoingDoc.Document, UserRole.HeadOfDepartment, _headOfDepartment, DocumentStatus.InWorkUnallocated));
                }
            }
        }

        private async Task UpdateHeadOfDepartmentForInternalDocuments(UpdateDocumentHeadOfDepartmentCommand request)
        {
            var internalDocIds = request.DocumentInfo
                                        .Where(doc => doc.DocType == (int)DocumentType.Internal)
                                        .Select(doc => doc.RegistrationNumber)
                                        .ToList();

            foreach (var registrationNo in internalDocIds)
            {
                var internalDoc = await _dbContext.InternalDocuments
                    .Include(x => x.Document)
                    .FirstOrDefaultAsync(x => x.Document.RegistrationNumber == registrationNo);

                if (internalDoc != null)
                {
                    internalDoc.ReceiverDepartmentId = (int)_headOfDepartment.Id;
                }
            }
        }

        private async Task UpdateHeadOfDepartmentForIncomingDocuments(UpdateDocumentHeadOfDepartmentCommand request)
        {
            var incomingDocIds = request.DocumentInfo
                                        .Where(doc => doc.DocType == (int)DocumentType.Incoming)
                                        .Select(doc => doc.RegistrationNumber)
                                        .ToList();

            foreach (var registrationNo in incomingDocIds)
            {
                var foundIncomingDocument = await _dbContext.IncomingDocuments
                    .Include(x => x.Document)
                    .FirstOrDefaultAsync(x => x.Document.RegistrationNumber == registrationNo);

                if (foundIncomingDocument != null)
                {
                    foundIncomingDocument.Document.Status = DocumentStatus.InWorkUnallocated;
                    foundIncomingDocument.RecipientId = (int)_headOfDepartment.Id;
                    foundIncomingDocument.WorkflowHistory.Add(WorkflowHistoryFactory.Create(foundIncomingDocument.Document, UserRole.HeadOfDepartment, _headOfDepartment, DocumentStatus.InWorkUnallocated));
                }
            }
        }
    }
}
