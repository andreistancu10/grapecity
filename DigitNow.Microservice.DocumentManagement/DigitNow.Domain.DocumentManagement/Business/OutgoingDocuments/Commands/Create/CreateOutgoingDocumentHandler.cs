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
    private readonly IOutgoingDocumentService _outgoingDocumentService;

    public CreateOutgoingDocumentHandler(DocumentManagementDbContext dbContext, 
        IMapper mapper, 
        IDocumentService service,
        IOutgoingDocumentService outgoingDocumentService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _service = service;
        _outgoingDocumentService = outgoingDocumentService;
    }

    public async Task<ResultObject> Handle(CreateOutgoingDocumentCommand request, CancellationToken cancellationToken)
    {
        var newOutgoingDocument = _mapper.Map<OutgoingDocument>(request);
        newOutgoingDocument.CreationDate = DateTime.Now;

        newOutgoingDocument = await _outgoingDocumentService.CreateAsync(newOutgoingDocument, cancellationToken);

        await AttachConnectedDocuments(request, newOutgoingDocument, cancellationToken);
        
        newOutgoingDocument.WorkflowHistory.Add(
            new WorkflowHistory
            {
                RecipientType = (int)UserRole.HeadOfDepartment,
                RecipientId = request.RecipientId,
                Status = (int)Status.inWorkUnallocated,
                CreationDate = DateTime.Now,
                RegistrationNumber = newOutgoingDocument.Document.RegistrationNumber
            });

        await _dbContext.SaveChangesAsync();

        await _service.AssignRegistrationNumberAsync(newOutgoingDocument.DocumentId);

        return ResultObject.Created(newOutgoingDocument.Id);
    }
    
    private async Task AttachConnectedDocuments(CreateOutgoingDocumentCommand request, OutgoingDocument outgoingDocumentForCreation, CancellationToken cancellationToken)
    {
        if (request.ConnectedDocumentIds.Any())
        {
            var connectedDocuments = await _dbContext.OutgoingDocuments
                .Include(x => x.Document)
                .Where(x => request.ConnectedDocumentIds.Contains(x.Document.RegistrationNumber)).ToListAsync(cancellationToken);

            foreach (var connectedDocument in connectedDocuments)
            {
                outgoingDocumentForCreation.ConnectedDocuments
                    .Add(new ConnectedDocument { RegistrationNumber = connectedDocument.Document.RegistrationNumber, DocumentType = DocumentType.Outgoing });
            }
        }
    }
}