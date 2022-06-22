using AutoMapper;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;

namespace DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Commands.Update;

public class UpdateOutgoingDocumentHandler : ICommandHandler<UpdateOutgoingDocumentCommand, ResultObject>
{
    private readonly DocumentManagementDbContext _dbContext;
    private readonly IMapper _mapper;

    public UpdateOutgoingDocumentHandler(DocumentManagementDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<ResultObject> Handle(UpdateOutgoingDocumentCommand request, CancellationToken cancellationToken)
    {
        var outgoingDocFromDb = await _dbContext.OutgoingDocuments.Include(cd => cd.ConnectedDocuments)
            .FirstOrDefaultAsync(doc => doc.Id == request.Id, cancellationToken);

        if (outgoingDocFromDb is null)
            return ResultObject.Error(new ErrorMessage
            {
                Message = $"Outgoing Document with id {request.Id} does not exist.",
                TranslationCode = "document-management.backend.update.validation.entityNotFound",
                Parameters = new object[] { request.Id }
            });

        RemoveConnectedDocs(request, outgoingDocFromDb);
        AddConnectedDocs(request, outgoingDocFromDb);

        _mapper.Map(request, outgoingDocFromDb);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return ResultObject.Ok(outgoingDocFromDb.Id);
    }

    private void AddConnectedDocs(UpdateOutgoingDocumentCommand request, OutgoingDocument outgoingDocFromDb)
    {
        var outgoingDocIds = outgoingDocFromDb.ConnectedDocuments.Select(cd => cd.RegistrationNumber).ToList();
        var idsToAdd = request.ConnectedDocumentIds.Except(outgoingDocIds);

        if (idsToAdd.Any())
        {
            var connectedDocuments = _dbContext.OutgoingDocuments
                .Where(doc => idsToAdd
                    .Contains(doc.RegistrationNumber))
                .ToList();

            foreach (var doc in connectedDocuments)
            {
                outgoingDocFromDb.ConnectedDocuments
                    .Add(new ConnectedDocument {  RegistrationNumber = doc.RegistrationNumber, DocumentType = DocumentType.Outgoing });
            }
        }
    }

    private void RemoveConnectedDocs(UpdateOutgoingDocumentCommand request, OutgoingDocument outgoingDocFromDb)
    {
        var outgoingDocIds = outgoingDocFromDb.ConnectedDocuments.Select(cd => cd.RegistrationNumber).ToList();
        var idsToRemove = outgoingDocIds.Except(request.ConnectedDocumentIds);

        if (idsToRemove.Any())
        {
            _dbContext.ConnectedDocuments
                .RemoveRange(outgoingDocFromDb.ConnectedDocuments
                    .Where(cd => idsToRemove
                        .Contains(cd.RegistrationNumber))
                    .ToList());
        }
    }
}