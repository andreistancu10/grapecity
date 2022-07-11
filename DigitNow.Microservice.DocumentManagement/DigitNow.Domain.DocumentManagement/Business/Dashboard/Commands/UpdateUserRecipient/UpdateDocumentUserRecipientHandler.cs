using DigitNow.Adapters.MS.Identity;
using DigitNow.Adapters.MS.Identity.Poco;
using DigitNow.Domain.DocumentManagement.Business.Common.Factories;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
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
            var outgoingDocuments = await _dbContext.OutgoingDocuments
                    .Include(x => x.Document)
                    .Where(x => documentIds.Contains(x.DocumentId))
                    .ToListAsync();

            foreach (var outgoingDocument in outgoingDocuments)
            {
                outgoingDocument.RecipientId = (int)_user.Id;
                outgoingDocument.Document.Status = _status;

                outgoingDocument.WorkflowHistory.Add(WorkflowHistoryFactory.Create(outgoingDocument.Document, _isHeadOfDepartment ? UserRole.HeadOfDepartment : UserRole.Functionary, _user, _status));
            }
        }

        private async Task UpdateRecipientForInternalDocuments(List<long> documentIds)
        {
            var documents = await _dbContext.InternalDocuments
                    .Include(x => x.Document)
                    .Where(x => documentIds.Contains(x.DocumentId))
                    .ToListAsync();

            foreach (var document in documents)
            {
                document.ReceiverDepartmentId = (int)_user.Id;     
            }
        }

        private async Task UpdateRecipientForIncomingDocuments(List<long> documentIds)
        {
            var incomingDocuments = await _dbContext.IncomingDocuments
                    .Include(x => x.Document)
                    .Where(x => documentIds.Contains(x.DocumentId))
                    .ToListAsync();

            foreach (var incomingDocument in incomingDocuments)
            {
                incomingDocument.RecipientId = (int)_user.Id;
                incomingDocument.Document.Status = _status;

                incomingDocument.WorkflowHistory.Add(WorkflowHistoryFactory.Create(incomingDocument.Document, _isHeadOfDepartment ? UserRole.HeadOfDepartment : UserRole.Functionary, _user, _status));
            }
        }
    }
}
