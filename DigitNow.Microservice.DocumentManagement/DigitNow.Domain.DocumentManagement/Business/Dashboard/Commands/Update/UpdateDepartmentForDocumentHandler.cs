using DigitNow.Adapters.MS.Identity;
using DigitNow.Adapters.MS.Identity.Poco;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.IncomingDocuments;
using DigitNow.Domain.DocumentManagement.Data.WorkflowHistories;
using DigitNow.Domain.DocumentManagement.extensions.Autocorrect;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Dashboard.Commands.Update
{
    public class UpdateDepartmentForDocumentHandler : ICommandHandler<UpdateDepartmentForDocumentCommand, ResultObject>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IIdentityAdapterClient _client;
        private User _headOfDepartment;

        public UpdateDepartmentForDocumentHandler(DocumentManagementDbContext dbContext, IIdentityAdapterClient client)
        {
            _dbContext = dbContext;
            _client = client;
        }
        public async Task<ResultObject> Handle(UpdateDepartmentForDocumentCommand request, CancellationToken cancellationToken)
        {

            var users = await _client.GetUsersByDepartmentIdAsync(request.DepartmentId);
            _headOfDepartment = users.Users.FirstOrDefault(x => x.TenantRoles.Contains((long)UserRole.HeadOfDepartment));
            
            var incomingDocIds = request.DocumentInfo
                                        .Where(doc => doc.DocType == (int)DocumentType.Incoming)
                                        .Select(doc => doc.RegistrationNumber)
                                        .ToList();

            if (incomingDocIds.Any())
                await UpdateDepartmentForIncomingDocs(incomingDocIds, request.DepartmentId);

            var internalDocIds = request.DocumentInfo
                                        .Where(doc => doc.DocType == (int)DocumentType.Internal)
                                        .Select(doc => doc.RegistrationNumber)
                                        .ToList();

            if (internalDocIds.Any())
                await UpdateDepartmentForInternalDocsAsync(incomingDocIds, request.DepartmentId);


            await _dbContext.SaveChangesAsync();

            return new ResultObject(ResultStatusCode.Ok);
        }

        private async Task UpdateDepartmentForInternalDocsAsync(List<int> incomingDocIds, int departmentId)
        {
            foreach (var registrationNo in incomingDocIds)
            {
                var internalDoc = await _dbContext.InternalDocuments.FirstOrDefaultAsync(doc => doc.RegistrationNumber == registrationNo);

                if (internalDoc != null)
                    internalDoc.ReceiverDepartmentId = _headOfDepartment == null ? 
                            internalDoc.ReceiverDepartmentId : (int)_headOfDepartment.Id;
            }
        }

        private async Task UpdateDepartmentForIncomingDocs(List<int> incomingDocIds, int departmentId)
        {
            foreach (var registrationNo in incomingDocIds)
            {
                var doc = await _dbContext.IncomingDocuments.FirstOrDefaultAsync(doc => doc.RegistrationNumber == registrationNo);
                if (doc != null)
                {
                    doc.RecipientId = _headOfDepartment == null ? doc.RecipientId : (int)_headOfDepartment.Id;
                    CreateWorkflowRecord(doc);
                }
            }
        }

        private void CreateWorkflowRecord(IncomingDocument doc)
        {
            if (_headOfDepartment == null) return;

            doc.WorkflowHistory.Add(
                new WorkflowHistory()
                {
                    RecipientType = (int)UserRole.HeadOfDepartment,
                    RecipientId = (int)_headOfDepartment.Id,
                    RecipientName = _headOfDepartment.FormatUserNameByRole(UserRole.HeadOfDepartment),
                    Status = (int)Status.in_work_unallocated,
                    CreationDate = DateTime.Now
                });
        }
    }
}
