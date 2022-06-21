using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Contracts.Documents;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Documents;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.InternalDocuments.Commands.Create;

public class CreateInternalDocumentHandler : ICommandHandler<CreateInternalDocumentCommand, ResultObject>
{
    private readonly DocumentManagementDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IDocumentService _service;

    public CreateInternalDocumentHandler(DocumentManagementDbContext dbContext, IMapper mapper, IDocumentService service)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _service = service;
    }

    public async Task<ResultObject> Handle(CreateInternalDocumentCommand request, CancellationToken cancellationToken)
    {
        var internalDocumentForCreation =
            _mapper.Map<InternalDocument>(request);
        internalDocumentForCreation.CreatedAt = DateTime.Now;

        await _service.AssignRegistrationNumberAsync(internalDocumentForCreation);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return ResultObject.Created(internalDocumentForCreation.Id);
    }
}