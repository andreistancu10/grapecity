using DigitNow.Adapters.MS.Identity;
using DigitNow.Adapters.MS.Identity.Poco;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.extensions.Autocorrect;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Dashboard.Commands.Update
{
    public class UpdateDocumentRecipientHandler : ICommandHandler<UpdateDocumentRecipientCommand, ResultObject>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IIdentityAdapterClient _identityAdapterClient;
        private User _headOfDepartment;

        public UpdateDocumentRecipientHandler(DocumentManagementDbContext dbContext, IIdentityAdapterClient identityAdapterClient)
        {
            _dbContext = dbContext;
            _identityAdapterClient = identityAdapterClient;
        }
        public async Task<ResultObject> Handle(UpdateDocumentRecipientCommand request, CancellationToken cancellationToken)
        {
            var users = await _identityAdapterClient.GetUsersByDepartmentIdAsync(request.DepartmentId);
            _headOfDepartment = users.Users.FirstOrDefault(x => x.Roles.Contains((long)UserRole.HeadOfDepartment));

            if (_headOfDepartment == null)
                return ResultObject.Error(new ErrorMessage
                {
                    Message = $"No responsible for department with id {request.DepartmentId} was found.",
                    TranslationCode = "catalog.headOfdepartment.backend.update.validation.entityNotFound",
                    Parameters = new object[] { request.DepartmentId }
                });

            await UpdateDocumentRecipients(request);
            await _dbContext.SaveChangesAsync();

            return new ResultObject(ResultStatusCode.Ok);
        }

        private async Task UpdateDocumentRecipients(UpdateDocumentRecipientCommand request)
        {
            await UpdateRecipientForIncomingDocuments(request);
            await UpdateRecipientForInternalDocuments(request);
            await UpdateRecipientForOutgoingDocuments(request);
        }

        private async Task UpdateRecipientForOutgoingDocuments(UpdateDocumentRecipientCommand request)
        {
            var outgoingDocIds = request.DocumentInfo
                                        .Where(doc => doc.DocType == (int)DocumentType.Internal)
                                        .Select(doc => doc.RegistrationNumber)
                                        .ToList();

            foreach (var registrationNo in outgoingDocIds)
            {
                var outgoingDoc = await _dbContext.OutgoingDocuments.FirstOrDefaultAsync(doc => doc.RegistrationNumber == registrationNo);

                if (outgoingDoc != null)
                    outgoingDoc.RecipientId = (int)_headOfDepartment.Id;
            }
        }

        private async Task UpdateRecipientForInternalDocuments(UpdateDocumentRecipientCommand request)
        {
            var internalDocIds = request.DocumentInfo
                                        .Where(doc => doc.DocType == (int)DocumentType.Internal)
                                        .Select(doc => doc.RegistrationNumber)
                                        .ToList();

            foreach (var registrationNo in internalDocIds)
            {
                var internalDoc = await _dbContext.InternalDocuments.FirstOrDefaultAsync(doc => doc.RegistrationNumber == registrationNo);

                if (internalDoc != null)
                    internalDoc.ReceiverDepartmentId = (int)_headOfDepartment.Id;
            }
        }

        private async Task UpdateRecipientForIncomingDocuments(UpdateDocumentRecipientCommand request)
        {
            var incomingDocIds = request.DocumentInfo
                                        .Where(doc => doc.DocType == (int)DocumentType.Incoming)
                                        .Select(doc => doc.RegistrationNumber)
                                        .ToList();

            foreach (var registrationNo in incomingDocIds)
            {
                var doc = await _dbContext.IncomingDocuments.FirstOrDefaultAsync(doc => doc.RegistrationNumber == registrationNo);
                if (doc != null)
                {
                    doc.RecipientId = (int)_headOfDepartment.Id;
                    CreateWorkflowRecord(doc);
                }
            }
        }

        private void CreateWorkflowRecord(IncomingDocument doc)
        {
            doc.WorkflowHistory.Add(
                new WorkflowHistory()
                {
                    RecipientType = (int)UserRole.HeadOfDepartment,
                    RecipientId = (int)_headOfDepartment.Id,
                    RecipientName = _headOfDepartment.FormatUserNameByRole(UserRole.HeadOfDepartment),
                    Status = (int)Status.inWorkUnallocated,
                    CreationDate = DateTime.Now,
                    RegistrationNumber = doc.RegistrationNumber
                });
        }
    }
}
