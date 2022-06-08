using System.Collections.Generic;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.ConnectedDocuments;
using DigitNow.Domain.DocumentManagement.Data.OutgoingDocuments;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Commands.Create
{
    public class CreateOutgoingDocumentHandler : ICommandHandler<CreateOutgoingDocumentCommand, ResultObject>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IMapper _mapper;

        public CreateOutgoingDocumentHandler(DocumentManagementDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ResultObject> Handle(CreateOutgoingDocumentCommand request, CancellationToken cancellationToken)
        {
            OutgoingDocument incomingDocumentForCreation = _mapper.Map<OutgoingDocument>(request);

            if (request.ConnectedDocumentIds.Any())
            {
                List<OutgoingDocument> connectedDocuments = await _dbContext.OutgoingDocuments.ToListAsync(cancellationToken);
            }

            await _dbContext.OutgoingDocuments.AddAsync(incomingDocumentForCreation, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return ResultObject.Created(incomingDocumentForCreation.Id);
        }
    }
}
