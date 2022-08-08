using AutoMapper;
using DigitNow.Adapters.MS.Catalog;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Documents.Commands.CreateDocumentDeliveryDetails
{
    public class CreateDocumentDeliveryDetailsHandler : ICommandHandler<CreateDocumentDeliveryDetailsCommand, ResultObject>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ICatalogAdapterClient _catalogAdapterClient;
        private readonly IIdentityService _identityService;

        public CreateDocumentDeliveryDetailsHandler(
            DocumentManagementDbContext dbContext, 
            IMapper mapper, 
            ICatalogAdapterClient catalogAdapterClient,
            IIdentityService identityService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _catalogAdapterClient = catalogAdapterClient;
            _identityService = identityService;
        }

        public async Task<ResultObject> Handle(CreateDocumentDeliveryDetailsCommand request, CancellationToken cancellationToken)
        {
            var deliveryDetails = _mapper.Map<DeliveryDetail>(request);
            var document = await _dbContext.Documents.FirstAsync(x => x.Id == request.DocumentId, cancellationToken);

            switch (document.DocumentType)
            {
                case DocumentType.Incoming:
                    await CreateDeliveryDetails<IncomingDocument>(document, deliveryDetails, cancellationToken);
                    break;
                case DocumentType.Outgoing:
                    await CreateDeliveryDetails<OutgoingDocument>(document, deliveryDetails, cancellationToken);
                    break;
                default:
                    return new ResultObject(ResultStatusCode.Error);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new ResultObject(ResultStatusCode.Ok);
        }

        private async Task CreateDeliveryDetails<T>(Document document, DeliveryDetail deliveryDetails, CancellationToken token) where T : VirtualDocument
        {
            var virtualDocument = await _dbContext.Set<T>().AsQueryable()
                .FirstOrDefaultAsync(x => x.DocumentId == document.Id, token);

            var shippableDocument = virtualDocument as IShippable;
            if (shippableDocument == null) throw new InvalidCastException($"Cannot convert from {nameof(VirtualDocument)} to {nameof(IShippable)}");

            shippableDocument.DeliveryDetails = deliveryDetails;

            var departmentToReceiveDocument = await _catalogAdapterClient.GetDepartmentByCodeAsync(UserDepartment.Registry.Code, token);

            document.DestinationDepartmentId = departmentToReceiveDocument.Id;
            document.RecipientId = await _identityService.GetHeadOfDepartmentUserIdAsync(departmentToReceiveDocument.Id, token);
            document.Status = DocumentStatus.Finalized;

            if (deliveryDetails.DeliveryMode == (int)TransmissionMode.DirectTransmission && document.DocumentType == DocumentType.Outgoing)
            {
                return;
            }

            var newWorkflowResponsible = new WorkflowHistoryLog
            {
                DocumentId = document.Id,
                DocumentStatus = DocumentStatus.Finalized,
                RecipientType = RecipientType.Department.Id,
                RecipientId = departmentToReceiveDocument.Id,
                RecipientName = $"Departamentul {departmentToReceiveDocument.Name}"
            };
            
            await _dbContext.WorkflowHistoryLogs.AddAsync(newWorkflowResponsible, token);
        }
    }
}
