using System;
using System.Linq;
using AutoMapper;
using HTSS.Platform.Core.CQRS;
using System.Threading;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Data;

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
        var newOutgoingDocument = _mapper.Map<OutgoingDocument>(request);
        newOutgoingDocument.CreationDate = DateTime.Now;

        await AttachConnectedDocuments(request, newOutgoingDocument, cancellationToken);
        
        newOutgoingDocument.WorkflowHistory.Add(
            new WorkflowHistory
            {
                RecipientType = (int)UserRole.HeadOfDepartment,
                RecipientId = request.RecipientId,
                Status = (int)Status.inWorkUnallocated,
                CreationDate = DateTime.Now,
                RegistrationNumber = newOutgoingDocument.RegistrationNumber
            });

        await _dbContext.SaveChangesAsync();

        await _service.AssignRegistrationNumberAsync(newOutgoingDocument);

        return ResultObject.Created(newOutgoingDocument.Id);
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
                    .Add(new ConnectedDocument { RegistrationNumber = doc.RegistrationNumber, DocumentType = DocumentType.Outgoing });
            }
        }
    }
}