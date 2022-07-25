
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Entities.DeliveryDetails;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Documents.Commands.CreateDocumentDeliveryDetails
{
    public class CreateDocumentDeliveryDetailsHandler : ICommandHandler<CreateDocumentDeliveryDetailsCommand, ResultObject>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IIncomingDocumentService _incomingDocumentService;
        private readonly IOutgoingDocumentService _outgoingDocumentService;

        public CreateDocumentDeliveryDetailsHandler(DocumentManagementDbContext dbContext, IMapper mapper, IIncomingDocumentService incomingDocumentService, IOutgoingDocumentService outgoingDocumentService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _incomingDocumentService = incomingDocumentService;
            _outgoingDocumentService = outgoingDocumentService;
        }

        public async Task<ResultObject> Handle(CreateDocumentDeliveryDetailsCommand request, CancellationToken cancellationToken)
        {
            var document = await _dbContext.Documents.FirstAsync(x => x.Id == request.DocumentId);
            var deliveryDetails = _mapper.Map<DeliveryDetail>(request);

            switch (document.DocumentType)
            {
                case DocumentType.Incoming:
                case DocumentType.Outgoing:
                    CreateDeliveryDetails(document, deliveryDetails, request, cancellationToken);
                    break;
                case DocumentType.Internal:
                    return InvalidActionForInternalDocuments(request);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new ResultObject(ResultStatusCode.Ok);
        }

        private async void CreateDeliveryDetails(Document document, DeliveryDetail deliveryDetails, CreateDocumentDeliveryDetailsCommand request, CancellationToken cancellationToken)
        {
            document.Status = DocumentStatus.Finalized;

            if (document.DocumentType == DocumentType.Incoming)
            {
                var foundIncomingDocument = await _incomingDocumentService.FindFirstAsync(request.DocumentId, cancellationToken);
                foundIncomingDocument.DeliveryDetails = deliveryDetails;
                return;
            }

            var foundOutgoingDocument = await _outgoingDocumentService.FindFirstAsync(request.DocumentId, cancellationToken);
            foundOutgoingDocument.DeliveryDetails = deliveryDetails;
        }

        private static ResultObject InvalidActionForInternalDocuments(CreateDocumentDeliveryDetailsCommand request)
        {
            return ResultObject.Error(new ErrorMessage
            {
                Message = $"Action not possible for internal document with id: {request.DocumentId}",
                TranslationCode = "catalog.backend.update.validation.invalidAction",
                Parameters = new object[] { request.DocumentId }
            });
        }
    }
}
