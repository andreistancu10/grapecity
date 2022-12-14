using DigitNow.Domain.Authentication.Client;
using DigitNow.Domain.Authentication.Contracts.Users.GetUsersByFilter;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.Factories;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Dashboard.Commands.Update
{
    public class UpdateDocumentHeadOfDepartmentHandler : ICommandHandler<UpdateDocumentHeadOfDepartmentCommand, ResultObject>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IIdentityService _identityService;
        private readonly IMailSenderService _mailSenderService;

        public UpdateDocumentHeadOfDepartmentHandler(
            DocumentManagementDbContext dbContext,
            IIdentityService identityService,
            IMailSenderService mailSenderService)
        {
            _dbContext = dbContext;
            _identityService = identityService;
            _mailSenderService = mailSenderService;
        }

        public async Task<ResultObject> Handle(UpdateDocumentHeadOfDepartmentCommand request, CancellationToken cancellationToken)
        {
            var headOfDepartment = await _identityService.GetHeadOfDepartmentUserAsync(request.DepartmentId, cancellationToken);
            if (headOfDepartment == null)
                return ResultObject.Error(new ErrorMessage
                {
                    Message = $"No responsible for department with id {request.DepartmentId} was found.",
                    TranslationCode = "catalog.headOfdepartment.backend.update.validation.entityNotFound",
                    Parameters = new object[] { request.DepartmentId }
                });

            await UpdateDocuments(request, headOfDepartment, cancellationToken);

            var documentIds = request.DocumentInfo.Select(x => x.DocumentId).ToList();
            await _mailSenderService.SendMail_OnSendBulkDocuments(headOfDepartment, documentIds, cancellationToken);

            return new ResultObject(ResultStatusCode.Ok);
        }

        private async Task UpdateDocuments(UpdateDocumentHeadOfDepartmentCommand request, UserModel headOfDepartment, CancellationToken token)
        {
            var requestedDocumentIds = request.DocumentInfo.Select(x => x.DocumentId);

            var foundDocuments = await _dbContext.Documents
                    .Include(x => x.WorkflowHistories)
                    .Where(x => requestedDocumentIds.Contains(x.Id))
                    .ToListAsync(token);
            
            foreach (var foundDocument in foundDocuments)
            {
                foundDocument.Status = foundDocument.DocumentType == DocumentType.Incoming ? DocumentStatus.InWorkUnallocated : DocumentStatus.New;
                foundDocument.DestinationDepartmentId = request.DepartmentId;
                foundDocument.RecipientId = headOfDepartment.Id;
                foundDocument.WorkflowHistories.Add(WorkflowHistoryLogFactory.Create(foundDocument, RecipientType.HeadOfDepartment, headOfDepartment, DocumentStatus.InWorkUnallocated));
            }

            await _dbContext.BulkUpdateAsync(foundDocuments);
            await _dbContext.SaveChangesAsync(token);
        }
    }
}
