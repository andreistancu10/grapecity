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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Dashboard.Commands.UpdateUserRecipient
{
    public class UpdateDocumentUserRecipientHandler: ICommandHandler<UpdateDocumentUserRecipientCommand, ResultObject>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IIdentityAdapterClient _identityAdapterClient;
        private User _user;
        private DocumentStatus _status;
        private bool _isHeadOfDepartment;

        public UpdateDocumentUserRecipientHandler(DocumentManagementDbContext dbContext, IIdentityAdapterClient identityAdapterClient)
        {
            _dbContext = dbContext;
            _identityAdapterClient = identityAdapterClient;
        }
        public async Task<ResultObject> Handle(UpdateDocumentUserRecipientCommand request, CancellationToken cancellationToken)
        {
            _user = await _identityAdapterClient.GetUserByIdAsync(request.UserId, cancellationToken);

            if (_user == null)
                return ResultObject.Error(new ErrorMessage
                {
                    Message = $"No responsible with id {request.UserId} was found.",
                    TranslationCode = "catalog.user.backend.update.validation.entityNotFound",
                    Parameters = new object[] { request.UserId }
                });

            _isHeadOfDepartment = _user.Roles.Contains((long)UserRole.HeadOfDepartment);
            _status = _isHeadOfDepartment ? DocumentStatus.InWorkDelegatedUnallocated : DocumentStatus.InWorkDelegated;

            await UpdateRecipientForIncomingDocuments(request.RegistrationNumbers);

            await UpdateRecipientForInternalDocuments(request.RegistrationNumbers);

            await UpdateRecipientForOutgoingDocuments(request.RegistrationNumbers);

            await _dbContext.SaveChangesAsync();

            return new ResultObject(ResultStatusCode.Ok);
        }

        private async Task UpdateRecipientForOutgoingDocuments(List<long> documentIds)
        {
            foreach (var documentId in documentIds)
            {
                var doc = await _dbContext.OutgoingDocuments
                    .Include(x => x.Document)
                    .FirstOrDefaultAsync(x => x.DocumentId == documentId);

                if (doc != null)
                {
                    doc.RecipientId = (int)_user.Id;
                    CreateWorkflowRecord(doc);
                }
            }
        }

        private async Task UpdateRecipientForInternalDocuments(List<long> documentIds)
        {
            foreach (var documentId in documentIds)
            {
                var internalDoc = await _dbContext.InternalDocuments
                    .Include(x => x.Document)
                    .FirstOrDefaultAsync(x => x.DocumentId == documentId);

                if (internalDoc != null)
                    internalDoc.ReceiverDepartmentId = (int)_user.Id;     
            }
        }

        private async Task UpdateRecipientForIncomingDocuments(List<long> documentIds)
        {
            foreach (var documentId in documentIds)
            {
                var doc = await _dbContext.IncomingDocuments
                    .Include(x => x.Document)
                    .FirstOrDefaultAsync(x => x.DocumentId == documentId);

                if (doc != null)
                {
                    doc.RecipientId = (int)_user.Id;
                    CreateWorkflowRecord(doc);
                }
            }
        }

        private void CreateWorkflowRecord(VirtualDocument document)
        {
            var type = document.GetType();
            dynamic doc;
            if (type.Name.Equals("IncomingDocument"))
            {
                doc = (IncomingDocument)document;
            }
            else
            {
                doc = (OutgoingDocument)document;
            }
            
            doc.Document.Status = _status;

            doc.WorkflowHistory.Add(
                new WorkflowHistory()
                {
                    RecipientType = _isHeadOfDepartment ? (int)UserRole.HeadOfDepartment : (int)UserRole.Functionary,
                    RecipientId = (int)_user.Id,
                    RecipientName =_user.FormatUserNameByRole(_isHeadOfDepartment ? UserRole.HeadOfDepartment : UserRole.Functionary),
                    Status = _status,
                    CreationDate = DateTime.Now,
                    RegistrationNumber = doc.Document.RegistrationNumber
                });
        }
    }
}
