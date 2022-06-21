using AutoMapper;
using DigitNow.Domain.DocumentManagement.Contracts.Documents;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.WorkflowHistories;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HTSS.Platform.Core.Errors;
using DigitNow.Domain.DocumentManagement.Data.ConnectedDocuments;
using DigitNow.Domain.DocumentManagement.Data.Documents;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;

namespace DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Commands.Create;

public class CreateIncomingDocumentHandler : ICommandHandler<CreateIncomingDocumentCommand, ResultObject>
{
    private readonly DocumentManagementDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IDocumentService _service;

    public CreateIncomingDocumentHandler(DocumentManagementDbContext dbContext, IMapper mapper, IDocumentService service)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _service = service;
    }

    public async Task<ResultObject> Handle(CreateIncomingDocumentCommand request, CancellationToken cancellationToken)
    {
        var newIncomingDocument = _mapper.Map<IncomingDocument>(request);
        newIncomingDocument.RegistrationDate = DateTime.Now;
        newIncomingDocument.CreatedAt = DateTime.Now;        
        try
        {
            await AttachConnectedDocuments(request, newIncomingDocument, cancellationToken);
            await _service.AssignRegistrationNumberAsync(newIncomingDocument);

            newIncomingDocument.WorkflowHistory.Add(
            new WorkflowHistory()
            {
                RecipientType = (int)UserRole.HeadOfDepartment,
                RecipientId = request.RecipientId,
                Status = (int)Status.inWorkUnallocated,
                CreationDate = DateTime.Now,
                RegistrationNumber = newIncomingDocument.RegistrationNumber
            });

            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return ResultObject.Error(new ErrorMessage()
            {
                Message = ex.InnerException.Message
            });
        }

        return ResultObject.Created(newIncomingDocument.Id);
    }

    private async Task AttachConnectedDocuments(CreateIncomingDocumentCommand request, IncomingDocument incomingDocumentForCreation, CancellationToken cancellationToken)
    {
        if (request.ConnectedDocumentIds.Any())
        {
            var connectedDocuments = await _dbContext.IncomingDocuments
                .Where(doc => request.ConnectedDocumentIds.Contains(doc.RegistrationNumber)).ToListAsync(cancellationToken: cancellationToken);

            foreach (var doc in connectedDocuments)
            {
                incomingDocumentForCreation.ConnectedDocuments
                    .Add(new ConnectedDocument { RegistrationNumber = doc.RegistrationNumber, DocumentType = DocumentType.Incoming, ChildDocumentId = doc.Id });
            }
        }
    }
}