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
            var parallelResult = await Task.WhenAll(
                UpdateRecipientForIncomingDocuments(request),
                UpdateRecipientForInternalDocuments(request),
                UpdateRecipientForOutgoingDocuments(request)
            );

            if (parallelResult.Any(x => !x))
            {
                throw new InvalidOperationException("Cannot set department for requested documents!");
            }
        }

        private async Task<bool> UpdateRecipientForOutgoingDocuments(UpdateDocumentRecipientCommand request)
        {
            var outgoingDocIds = request.DocumentInfo
                                        .Where(doc => doc.DocType == (int)DocumentType.Internal)
                                        .Select(doc => doc.RegistrationNumber)
                                        .ToList();

            foreach (var registrationNo in outgoingDocIds)
            {
                var outgoingDoc = await _dbContext.OutgoingDocuments
                    .Include(x => x.Document)
                    .FirstOrDefaultAsync(doc => doc.RegistrationNumber == registrationNo);

                if (outgoingDoc != null)
                {
                    outgoingDoc.RecipientId = (int)_headOfDepartment.Id;
                    WorkflowHistoryFactory.Create(outgoingDoc.Document, UserRole.HeadOfDepartment, _headOfDepartment, Status.inWorkUnallocated);
                }
            }

            return true;
        }

        private async Task<bool> UpdateRecipientForInternalDocuments(UpdateDocumentRecipientCommand request)
        {
            var internalDocIds = request.DocumentInfo
                                        .Where(doc => doc.DocType == (int)DocumentType.Internal)
                                        .Select(doc => doc.RegistrationNumber)
                                        .ToList();

            foreach (var registrationNo in internalDocIds)
            {
                var internalDoc = await _dbContext.InternalDocuments
                    .Include(x => x.Document)
                    .FirstOrDefaultAsync(doc => doc.RegistrationNumber == registrationNo);

                if (internalDoc != null)
                {
                    internalDoc.ReceiverDepartmentId = (int)_headOfDepartment.Id;
                    WorkflowHistoryFactory.Create(internalDoc.Document, UserRole.HeadOfDepartment, _headOfDepartment, Status.inWorkUnallocated);
                }
            }

            return true;
        }

        private async Task<bool> UpdateRecipientForIncomingDocuments(UpdateDocumentRecipientCommand request)
        {
            var incomingDocIds = request.DocumentInfo
                                        .Where(doc => doc.DocType == (int)DocumentType.Incoming)
                                        .Select(doc => doc.RegistrationNumber)
                                        .ToList();

            foreach (var registrationNo in incomingDocIds)
            {
                var foundIncomingDocument = await _dbContext.IncomingDocuments
                    .Include(x => x.Document)
                    .FirstOrDefaultAsync(doc => doc.RegistrationNumber == registrationNo);

                if (foundIncomingDocument != null)
                {
                    foundIncomingDocument.RecipientId = (int)_headOfDepartment.Id;
                    WorkflowHistoryFactory.Create(foundIncomingDocument.Document, UserRole.HeadOfDepartment, _headOfDepartment, Status.inWorkUnallocated);
                }
            }

            return true;
        }
    }
}
