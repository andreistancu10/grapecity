using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.IncomingDocuments;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Dashboard.Commands.Update
{
    public class UpdateDepartmentForDocumentHandler : ICommandHandler<UpdateDocDepartmentCommand, ResultObject>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IMapper _mapper;

        public UpdateDepartmentForDocumentHandler(DocumentManagementDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<ResultObject> Handle(UpdateDocDepartmentCommand request, CancellationToken cancellationToken)
        {

            var incomingDocIds = request.DocumentInfo
                                        .Where(doc => doc.DocType == 1)
                                        .Select(doc => doc.RegistrationNumber)
                                        .ToList();


            if (incomingDocIds.Any())
                await UpdateDepartmentForIncomingDocs(incomingDocIds, request.DepartmentId);

            var internalDocIds = request.DocumentInfo
                                        .Where(doc => doc.DocType == 2)
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
                    internalDoc.DepartmentId = departmentId;
            }
        }

        private async Task UpdateDepartmentForIncomingDocs(List<int> incomingDocIds, int departmentId)
        {
            foreach (var registrationNo in incomingDocIds)
            {
                var doc = await _dbContext.IncomingDocuments.FirstOrDefaultAsync(doc => doc.RegistrationNumber == registrationNo);
                if (doc != null)
                {
                    doc.RecipientId = departmentId;
                    CreateWorkflowRecord(doc);
                }
            }
        }

        private void CreateWorkflowRecord(IncomingDocument doc)
        {
        }
    }
}
