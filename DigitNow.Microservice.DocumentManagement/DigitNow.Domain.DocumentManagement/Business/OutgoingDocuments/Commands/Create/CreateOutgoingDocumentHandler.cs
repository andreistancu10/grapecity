using System;
using System.Linq;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.OutgoingDocuments;
using HTSS.Platform.Core.CQRS;
using System.Threading;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Contracts.Documents;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data.OutgoingConnectedDocuments;
using DigitNow.Domain.DocumentManagement.Data.WorkflowHistories;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Commands.Create;

public class CreateOutgoingDocumentHandler : ICommandHandler<CreateOutgoingDocumentCommand, ResultObject>
{
    private readonly DocumentManagementDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IDocumentService _service;

    public CreateOutgoingDocumentHandler(DocumentManagementDbContext dbContext, IMapper mapper, IDocumentService service)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _service = service;
    }

    public async Task<ResultObject> Handle(CreateOutgoingDocumentCommand request, CancellationToken cancellationToken)
    {
        var outgoingDocumentForCreation = _mapper.Map<OutgoingDocument>(request);
        outgoingDocumentForCreation.CreationDate = DateTime.Now;

        await AttachConnectedDocuments(request, outgoingDocumentForCreation, cancellationToken);

        outgoingDocumentForCreation.WorkflowHistory.Add(
            new WorkflowHistory
            {
                RecipientType = (int)UserRole.HeadOfDepartment,
                RecipientId = request.RecipientId,
                Status = (int)Status.inWorkUnallocated,
                CreationDate = DateTime.Now,
                RegistrationNumber = outgoingDocumentForCreation.RegistrationNumber
            });

        await _service.AssignRegNumberAndSaveDocument(outgoingDocumentForCreation);

        return ResultObject.Created(outgoingDocumentForCreation.Id);
    }
    
    private async Task AttachConnectedDocuments(CreateOutgoingDocumentCommand request, OutgoingDocument outgoingDocumentForCreation, CancellationToken cancellationToken)
    {
        if (request.ConnectedDocumentIds.Any())
        {
            var connectedDocuments = await _dbContext.OutgoingDocuments
                .Where(doc => request.ConnectedDocumentIds.Contains(doc.RegistrationNumber)).ToListAsync(cancellationToken);

            foreach (var doc in connectedDocuments)
            {
                outgoingDocumentForCreation.ConnectedDocuments
                    .Add(new OutgoingConnectedDocument { RegistrationNumber = doc.RegistrationNumber, DocumentType = doc.DocumentTypeId });
            }
        }
    }
}