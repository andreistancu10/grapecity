using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.Factories;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Dashboard.Commands.UpdateUserRecipient
{
    public class UpdateDocumentUserRecipientHandler: ICommandHandler<UpdateDocumentUserRecipientCommand, ResultObject>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IIdentityService _identityService;
        private readonly IMailSenderService _mailSenderService;


        public UpdateDocumentUserRecipientHandler(
            DocumentManagementDbContext dbContext,
            IIdentityService identityService,
            IMailSenderService mailSenderService)
        {
            _dbContext = dbContext;
            _identityService = identityService;
            _mailSenderService = mailSenderService;
        }
        public async Task<ResultObject> Handle(UpdateDocumentUserRecipientCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _identityService.GetUserByIdAsync(_identityService.GetCurrentUserId(), cancellationToken);
            var targetUser = await _identityService.GetUserByIdAsync(request.UserId, cancellationToken);

            if (targetUser == null)
                return ResultObject.Error(new ErrorMessage
                {
                    Message = $"No responsible with id {request.UserId} was found.",
                    TranslationCode = "catalog.user.backend.update.validation.entityNotFound",
                    Parameters = new object[] { request.UserId }
                });

            if (!targetUser.Departments.Any())
                return ResultObject.Error(new ErrorMessage
                {
                    Message = $"The user with id {request.UserId} does not have any departments.",
                    TranslationCode = "catalog.user.backend.update.validation.departmentEntityNotFound",
                    Parameters = new object[] { request.UserId }
                });

            await UpdateDocumentsAsync(request.DocumentIds, targetUser, cancellationToken);
  
            await _mailSenderService.SendMail_DelegateDocumentToFunctionary(currentUser, targetUser, request.DocumentIds, cancellationToken);
            await _mailSenderService.SendMail_DelegateDocumentToFunctionarySupervisor(currentUser, targetUser, request.DocumentIds, cancellationToken);

            return new ResultObject(ResultStatusCode.Ok);
        }

        private async Task UpdateDocumentsAsync(List<long> documentIds, UserModel targetUser, CancellationToken token)
        {
            var foundDocuments = await _dbContext.Documents
                .Include(x => x.WorkflowHistories)
                .Where(x => documentIds.Contains(x.Id))
                .ToListAsync(token);

            foreach (var foundDocument in foundDocuments)
            {
                var isHeadOfDepartment = targetUser.Roles.Select(x => x.Code).Contains(RecipientType.HeadOfDepartment.Code);
                if (isHeadOfDepartment)
                {
                    foundDocument.Status = foundDocument.DocumentType == DocumentType.Incoming ? DocumentStatus.InWorkDelegatedUnallocated : DocumentStatus.New;
                }
                else
                {
                    foundDocument.Status = foundDocument.DocumentType == DocumentType.Incoming ? DocumentStatus.InWorkDelegated : DocumentStatus.New;
                }

                foundDocument.DestinationDepartmentId = targetUser.Departments.First().Id;
                foundDocument.RecipientId = targetUser.Id;

                var recipientType = isHeadOfDepartment ? RecipientType.HeadOfDepartment : RecipientType.Functionary;
                foundDocument.WorkflowHistories.Add(WorkflowHistoryLogFactory.Create(foundDocument, recipientType, targetUser, foundDocument.Status));
            }

            await _dbContext.BulkUpdateAsync(foundDocuments, token);
            await _dbContext.SaveChangesAsync(token);
        }
    }
}
