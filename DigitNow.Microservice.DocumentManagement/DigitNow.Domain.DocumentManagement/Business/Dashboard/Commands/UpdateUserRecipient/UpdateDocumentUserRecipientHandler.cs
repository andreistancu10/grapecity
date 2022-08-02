using DigitNow.Adapters.MS.Identity;
using DigitNow.Adapters.MS.Identity.Poco;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.Factories;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using Microsoft.EntityFrameworkCore;
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
        private readonly IIdentityService _identityService;
        private readonly IMailSenderService _mailSenderService;
        private User _currentUser;
        private User _delegatedUser;


        public UpdateDocumentUserRecipientHandler(
            DocumentManagementDbContext dbContext,
            IIdentityAdapterClient identityAdapterClient,
            IIdentityService identityService,
            IMailSenderService mailSenderService)
        {
            _dbContext = dbContext;
            _identityAdapterClient = identityAdapterClient;
            _identityService = identityService;
            _mailSenderService = mailSenderService;
        }
        public async Task<ResultObject> Handle(UpdateDocumentUserRecipientCommand request, CancellationToken cancellationToken)
        {

            _currentUser = await _identityAdapterClient.GetUserByIdAsync(_identityService.GetCurrentUserId(), cancellationToken);
            _delegatedUser = await _identityAdapterClient.GetUserByIdAsync(request.UserId, cancellationToken);

            if (_delegatedUser == null)
                return ResultObject.Error(new ErrorMessage
                {
                    Message = $"No responsible with id {request.UserId} was found.",
                    TranslationCode = "catalog.user.backend.update.validation.entityNotFound",
                    Parameters = new object[] { request.UserId }
                });

            await UpdateDocumentsAsync(request.DocumentIds, _delegatedUser, cancellationToken);

            await _mailSenderService.SendMail_DelegateDocumentToFunctionarySupervisor(_currentUser, _delegatedUser, cancellationToken);

            //await Task.WhenAll
            //(
            //    _mailSenderService.SendMail_DelegateDocumentToFunctionary(_currentUser, _delegatedUser, request.DocumentIds, cancellationToken),
            //    _mailSenderService.SendMail_DelegateDocumentToFunctionarySupervisor(_currentUser, _delegatedUser, cancellationToken)
            //);

            return new ResultObject(ResultStatusCode.Ok);
        }

        private async Task UpdateDocumentsAsync(List<long> documentIds, User targetUser, CancellationToken token)
        {
            var foundDocuments = await _dbContext.Documents
                .Where(x => documentIds.Contains(x.Id))
                .ToListAsync(token);

            var newWorkflowHistoryLogs = new List<WorkflowHistoryLog>();

            foreach (var foundDocument in foundDocuments)
            {
                var isHeadOfDepartment = targetUser.Roles.Contains(RecipientType.HeadOfDepartment.Code);
                if (isHeadOfDepartment)
                {
                    foundDocument.Status = DocumentStatus.InWorkDelegatedUnallocated;
                }
                else
                {
                    foundDocument.Status = DocumentStatus.InWorkDelegated;
                }
                
                foundDocument.DestinationDepartmentId = targetUser.Departments.FirstOrDefault();
                foundDocument.RecipientId = targetUser.Id;

                var recipientType = isHeadOfDepartment ? RecipientType.HeadOfDepartment : RecipientType.Functionary;

                newWorkflowHistoryLogs.Add(WorkflowHistoryLogFactory.Create(foundDocument.Id, recipientType, targetUser, foundDocument.Status));
            }

            await _dbContext.BulkUpdateAsync(foundDocuments, token);
            await _dbContext.BulkInsertAsync(newWorkflowHistoryLogs, token);
            await _dbContext.SaveChangesAsync(token);
        }
    }
}
