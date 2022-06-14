using System.Collections.Generic;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.IncomingDocuments;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Data.IncomingConnectedDocuments;

namespace DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Commands.Update
{
    public class UpdateIncomingDocumentHandler : ICommandHandler<UpdateIncomingDocumentCommand, ResultObject>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IMapper _mapper;

        public UpdateIncomingDocumentHandler(DocumentManagementDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<ResultObject> Handle(UpdateIncomingDocumentCommand request, CancellationToken cancellationToken)
        {
            IncomingDocument incomingDocFromDb = await _dbContext.IncomingDocuments.Include(cd => cd.ConnectedDocuments)
                                                    .FirstOrDefaultAsync(doc => doc.Id == request.Id, cancellationToken: cancellationToken);

            if (incomingDocFromDb is null)
                return ResultObject.Error(new ErrorMessage
                {
                    Message = $"Incoming Document with id {request.Id} does not exist.",
                    TranslationCode = "document-management.backend.update.validation.entityNotFound",
                    Parameters = new object[] { request.Id }
                });

            RemoveConnectedDocs(request, incomingDocFromDb);
            AddConnectedDocs(request, incomingDocFromDb);

            _mapper.Map(request, incomingDocFromDb);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ResultObject.Ok(incomingDocFromDb.Id);
        }

        private void AddConnectedDocs(UpdateIncomingDocumentCommand request, IncomingDocument incomingDocFromDb)
        {
            List<int> incomingDocIds = incomingDocFromDb.ConnectedDocuments.Select(cd => cd.RegistrationNumber).ToList();
            IEnumerable<int> idsToAdd = request.ConnectedDocumentIds.Except(incomingDocIds);

            if (idsToAdd.Any())
            {
                List<IncomingDocument> connectedDocuments = _dbContext.IncomingDocuments
                                                   .Where(doc => idsToAdd
                                                   .Contains(doc.RegistrationNumber))
                                                   .ToList();

                foreach (IncomingDocument doc in connectedDocuments)
                {
                    incomingDocFromDb.ConnectedDocuments
                        .Add(new IncomingConnectedDocument() { ChildIncomingDocumentId = doc.Id, RegistrationNumber = doc.RegistrationNumber, DocumentType = doc.DocumentTypeId });
                }
            }
        }

        private void RemoveConnectedDocs(UpdateIncomingDocumentCommand request, IncomingDocument incomingDocFromDb)
        {
            List<int> incomingDocIds = incomingDocFromDb.ConnectedDocuments.Select(cd => cd.RegistrationNumber).ToList();
            IEnumerable<int> idsToRemove = incomingDocIds.Except(request.ConnectedDocumentIds);

            if (idsToRemove.Any())
            {

                _dbContext.IncomingConnectedDocuments
                          .RemoveRange(incomingDocFromDb.ConnectedDocuments
                          .Where(cd => idsToRemove
                          .Contains(cd.RegistrationNumber))
                          .ToList());
            }
        }
    }
}
