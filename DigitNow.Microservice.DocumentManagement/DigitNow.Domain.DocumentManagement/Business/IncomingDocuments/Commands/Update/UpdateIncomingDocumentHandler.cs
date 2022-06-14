using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.ConnectedDocuments;
using DigitNow.Domain.DocumentManagement.Data.IncomingDocuments;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
            var incomingDocFromDb = await _dbContext.IncomingDocuments.Include(cd => cd.ConnectedDocuments)
                                                    .FirstOrDefaultAsync(doc => doc.Id == request.Id);

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

            await _dbContext.SaveChangesAsync();

            return ResultObject.Ok(incomingDocFromDb.Id);
        }

        private void AddConnectedDocs(UpdateIncomingDocumentCommand request, IncomingDocument incomingDocFromDb)
        {
            var incomingDocIds = incomingDocFromDb.ConnectedDocuments.Select(cd => cd.RegistrationNumber).ToList();
            var idsToAdd = request.ConnectedDocumentIds.Except(incomingDocIds);

            if (idsToAdd.Any())
            {
                var connectedDocuments = _dbContext.IncomingDocuments
                                                   .Where(doc => idsToAdd
                                                   .Contains(doc.RegistrationNumber))
                                                   .ToList();

                foreach (var doc in connectedDocuments)
                {
                    incomingDocFromDb.ConnectedDocuments
                        .Add(new ConnectedDocument() { ChildIncomingDocumentId = doc.Id, RegistrationNumber = doc.RegistrationNumber, DocumentType = doc.DocumentTypeId });
                }
            }
        }

        private void RemoveConnectedDocs(UpdateIncomingDocumentCommand request, IncomingDocument incomingDocFromDb)
        {
            var incomingDocIds = incomingDocFromDb.ConnectedDocuments.Select(cd => cd.RegistrationNumber).ToList();
            var idsToRemove = incomingDocIds.Except(request.ConnectedDocumentIds);

            if (idsToRemove.Any())
            {

                _dbContext.ConnectedDocuments
                          .RemoveRange(incomingDocFromDb.ConnectedDocuments
                          .Where(cd => idsToRemove
                          .Contains(cd.RegistrationNumber))
                          .ToList());
            }
        }
    }
}
