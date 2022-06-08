using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.ConnectedDocuments;
using DigitNow.Domain.DocumentManagement.Data.IncomingDocuments;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Commands.Create
{
    public class CreateIncomingDocumentHandler : ICommandHandler<CreateIncomingDocumentCommand, ResultObject>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IMapper _mapper;

        public CreateIncomingDocumentHandler(DocumentManagementDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<ResultObject> Handle(CreateIncomingDocumentCommand request, CancellationToken cancellationToken)
        {
            var incomingDocumentForCreation          = _mapper.Map<IncomingDocument>(request);
            incomingDocumentForCreation.CreationDate = DateTime.Now;

            if (request.ConnectedDocumentIds.Any())
            {
                var connectedDocuments = await _dbContext.IncomingDocuments
                    .Where(doc => request.ConnectedDocumentIds.Contains(doc.RegistrationNumber)).ToListAsync();

                foreach (var doc in connectedDocuments)
                {
                    incomingDocumentForCreation.ConnectedDocuments
                        .Add(new ConnectedDocument() { ChildIncomingDocumentId = doc.Id, RegistrationNumber = doc.RegistrationNumber, DocumentType = doc.DocumentTypeId });
                }
            }

            await _dbContext.IncomingDocuments.AddAsync(incomingDocumentForCreation);
            await _dbContext.SaveChangesAsync();

            return ResultObject.Created(incomingDocumentForCreation.Id);
        }
    }
}
