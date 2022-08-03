using AutoMapper;
using DigitNow.Adapters.MS.Catalog;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Entities.Documents.Abstractions;
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

            var departmentToReceiveDocument = default(long);

            if (document.DocumentType == DocumentType.Outgoing)
            {
                departmentToReceiveDocument = GetDestinationDepartmentFromHistory();
            }
            else
            {
                departmentToReceiveDocument = await GetDestinationDepartmentByCodeAsync(UserDepartment.Registry.Code, token);
            }

            document.DestinationDepartmentId = departmentToReceiveDocument;
            document.RecipientId = await _identityService.GetHeadOfDepartmentUserIdAsync(departmentToReceiveDocument, token);
            document.Status = DocumentStatus.Finalized;

            var department = await _catalogAdapterClient.GetDepartmentByIdAsync(departmentToReceiveDocument, token);

            var newWorkflowResponsible = new WorkflowHistoryLog
            {
                DocumentId = document.Id,
                DocumentStatus = DocumentStatus.Finalized,
                RecipientType = RecipientType.Department.Id,
                RecipientId = departmentToReceiveDocument,
                RecipientName = $"Departamentul {department.Name}"
            };
            
            await _dbContext.WorkflowHistoryLogs.AddAsync(newWorkflowResponsible, token);
        }

        private long GetDestinationDepartmentFromHistory()
        {
            return _dbContext.WorkflowHistoryLogs
                .Where(x => x.RecipientType == RecipientType.Department.Id)
                .OrderBy(x => x.CreatedAt)
                .First().RecipientId;
        }
        private async Task<long> GetDestinationDepartmentByCodeAsync(string code, CancellationToken token)
        {
            var department = await _catalogAdapterClient.GetDepartmentByCodeAsync(code, token);
            return department.Id;
        }
    }
}
