using DigitNow.Adapters.MS.Identity;
using DigitNow.Adapters.MS.Identity.Poco;
using DigitNow.Domain.Authentication.Client;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.Factories;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
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
        private readonly IAuthenticationClient _authenticationClient;
        private readonly IIdentityService _identityService;
        private readonly IMailSenderService _mailSenderService;
        private User _currentUser;
        private User _delegatedUser;
        private DocumentStatus _status;
        private bool _isHeadOfDepartment;

        public UpdateDocumentUserRecipientHandler(
            DocumentManagementDbContext dbContext,
            IIdentityAdapterClient identityAdapterClient,
            IAuthenticationClient authenticationClient,
            IIdentityService identityService,
            IMailSenderService mailSenderService)
        {
            _dbContext = dbContext;
            _identityAdapterClient = identityAdapterClient;
            _authenticationClient = authenticationClient;
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

            _isHeadOfDepartment = _delegatedUser.Roles.Contains(UserRole.HeadOfDepartment.Code);
            _status = _isHeadOfDepartment ? DocumentStatus.InWorkDelegatedUnallocated : DocumentStatus.InWorkDelegated;

            await UpdateDocuments(request.DocumentIds);
            await _dbContext.SaveChangesAsync();

            await Task.WhenAll
            (
                _mailSenderService.SendMail_DelegateDocumentToFunctionary(_currentUser, _delegatedUser, request.DocumentIds, cancellationToken),
                _mailSenderService.SendMail_DelegateDocumentToFunctionarySupervisor(_currentUser, _delegatedUser, cancellationToken)
            );

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
                }
            }
        }
        private async Task UpdateRecipient<T>(Document document) where T : VirtualDocument
        {
            var virtualDocument = await _dbContext.Set<T>().AsQueryable()
                .FirstOrDefaultAsync(x => x.DocumentId == document.Id);

            document.Status = _status;
            document.RecipientId = (int)_delegatedUser.Id;
            virtualDocument.WorkflowHistory.Add(WorkflowHistoryFactory.Create(_isHeadOfDepartment ? UserRole.HeadOfDepartment : UserRole.Functionary, _delegatedUser, _status));
        }
    }
}
