using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.InternalDocument.Commands.Create;

public class CreateInternalDocumentHandler : ICommandHandler<CreateInternalDocumentCommand, ResultObject>
{
    private readonly DocumentManagementDbContext _dbContext;
    private readonly IMapper _mapper;

    public CreateInternalDocumentHandler(DocumentManagementDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }


    public async Task<ResultObject> Handle(CreateInternalDocumentCommand request, CancellationToken cancellationToken)
    {
        var internalDocumentForCreation =
            _mapper.Map<Data.InternalDocument.InternalDocument>(request);
        internalDocumentForCreation.RegistrationDate = DateTime.Now;
        
        var currentMaxRegistrationNo = _dbContext.InternalDocuments.Count(doc => doc.RegistrationDate.Year == DateTime.Now.Year);
        internalDocumentForCreation.RegistrationNumber = ++currentMaxRegistrationNo;

        await _dbContext.InternalDocuments.AddAsync(internalDocumentForCreation, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return ResultObject.Created(internalDocumentForCreation.Id);
    }
}