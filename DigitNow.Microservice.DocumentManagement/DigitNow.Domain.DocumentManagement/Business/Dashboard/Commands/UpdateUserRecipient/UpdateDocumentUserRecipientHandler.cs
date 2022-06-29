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

            var incomingDocIds = request.DocumentInfo
                                          .Where(doc => doc.DocType == (int)DocumentType.Incoming)
                                          .Select(doc => doc.RegistrationNumber)
                                          .ToList();

            if (incomingDocIds.Any())
                await UpdateRecipientForIncomingDocuments(incomingDocIds);

            var internalDocIds = request.DocumentInfo
                                        .Where(doc => doc.DocType == (int)DocumentType.Internal)
                                        .Select(doc => doc.RegistrationNumber)
                                        .ToList();

            if (internalDocIds.Any())
                await UpdateRecipientForInternalDocuments(incomingDocIds);


            await _dbContext.SaveChangesAsync();

            return new ResultObject(ResultStatusCode.Ok);
        }

        private async Task UpdateRecipientForInternalDocuments(List<int> incomingDocIds)
        {
            foreach (var registrationNo in incomingDocIds)
            {
                var internalDoc = await _dbContext.InternalDocuments
                    .Include(x => x.Document)
                    .FirstOrDefaultAsync(x => x.Document.RegistrationNumber == registrationNo);

                if (internalDoc != null)
                    internalDoc.ReceiverDepartmentId = (int)_user.Id;     
            }
        }

        private async Task UpdateRecipientForIncomingDocuments(List<int> incomingDocIds)
        {
            foreach (var registrationNo in incomingDocIds)
            {
                var doc = await _dbContext.IncomingDocuments
                    .Include(x => x.Document)
                    .FirstOrDefaultAsync(x => x.Document.RegistrationNumber == registrationNo);

                if (doc != null)
                {
                    doc.RecipientId = (int)_user.Id;
                    CreateWorkflowRecord(doc);
                }
            }
        }

        private void CreateWorkflowRecord(IncomingDocument doc)
        {
            var isHeadOfDepartment = _user.Roles.Contains((long)UserRole.HeadOfDepartment);

            doc.WorkflowHistory.Add(
                new Data.WorkflowHistories.WorkflowHistory()
                {
                    RecipientType = isHeadOfDepartment ? (int)UserRole.HeadOfDepartment : (int)UserRole.Functionary,
                    RecipientId = (int)_user.Id,
                    RecipientName =_user.FormatUserNameByRole( isHeadOfDepartment ? UserRole.HeadOfDepartment : UserRole.Functionary),
                    Status = isHeadOfDepartment ? (int)DocumentStatus.InWorkDelegatedUnallocated : (int)DocumentStatus.InWorkDelegated,
                    CreationDate = DateTime.Now,
                    RegistrationNumber = doc.Document.RegistrationNumber
                });
        }
    }
}
