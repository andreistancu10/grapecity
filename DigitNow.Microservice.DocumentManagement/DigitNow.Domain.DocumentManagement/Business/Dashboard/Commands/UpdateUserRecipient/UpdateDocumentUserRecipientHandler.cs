using DigitNow.Adapters.MS.Identity;
using DigitNow.Adapters.MS.Identity.Poco;
using DigitNow.Domain.DocumentManagement.Business.Common.Factories;
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

            _isHeadOfDepartment = _user.Roles.Contains(RecipientType.HeadOfDepartment.Code);
            _status = _isHeadOfDepartment ? DocumentStatus.InWorkDelegatedUnallocated : DocumentStatus.InWorkDelegated;

            await UpdateDocuments(request.DocumentIds);

            await _dbContext.SaveChangesAsync();

            return new ResultObject(ResultStatusCode.Ok);
        }

        private async Task UpdateDocuments(List<long> documentIds)
        {
            foreach (var documentId in documentIds)
            {
                var document = await _dbContext.Documents.FirstAsync(x => x.Id == documentId);

                switch (document.DocumentType)
                {
                    case DocumentType.Incoming:
                        await UpdateRecipient<IncomingDocument>(document);
                        break;
                    case DocumentType.Internal:
                        await UpdateRecipient<InternalDocument>(document);
                        break;
                    case DocumentType.Outgoing:
                        await UpdateRecipient<OutgoingDocument>(document);
                        break;
                    default:
                        break;
                }
            }
        }
        private async Task UpdateRecipient<T>(Document document) where T : VirtualDocument
        {
            var virtualDocument = await _dbContext.Set<T>().AsQueryable()
                .FirstOrDefaultAsync(x => x.DocumentId == document.Id);

            document.Status = _status;
            document.RecipientId = (int)_user.Id;
            virtualDocument.WorkflowHistory.Add(WorkflowHistoryFactory.Create(_isHeadOfDepartment ? RecipientType.HeadOfDepartment : RecipientType.Functionary, _user, _status));
        }
    }
}
