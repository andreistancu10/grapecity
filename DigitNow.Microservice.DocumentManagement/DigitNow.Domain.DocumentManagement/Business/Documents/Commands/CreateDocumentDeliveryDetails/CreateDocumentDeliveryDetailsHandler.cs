using AutoMapper;
using DigitNow.Adapters.MS.Catalog;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Entities.DeliveryDetails;
using DigitNow.Domain.DocumentManagement.Data.Entities.Documents.Abstractions;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Documents.Commands.CreateDocumentDeliveryDetails
{
    public class CreateDocumentDeliveryDetailsHandler : ICommandHandler<CreateDocumentDeliveryDetailsCommand, ResultObject>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ICatalogAdapterClient _catalogAdapterClient;

        public CreateDocumentDeliveryDetailsHandler(DocumentManagementDbContext dbContext, IMapper mapper, ICatalogAdapterClient catalogAdapterClient)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _catalogAdapterClient = catalogAdapterClient;
        }

        public async Task<ResultObject> Handle(CreateDocumentDeliveryDetailsCommand request, CancellationToken cancellationToken)
        {
            var deliveryDetails = _mapper.Map<DeliveryDetail>(request);
            var document = await _dbContext.Documents.FirstAsync(x => x.Id == request.DocumentId);

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
                .FirstOrDefaultAsync(x => x.DocumentId == document.Id);

            var shippableDocument = virtualDocument as IShippable;
            if (shippableDocument == null) throw new InvalidCastException($"Cannot convert from {nameof(VirtualDocument)} to {nameof(IShippable)}");

            shippableDocument.DeliveryDetails = deliveryDetails;

            // TODO: Refactor this part after DestinationDepartmentId will be added on DocumentBase
            document.RecipientId = 0;
            document.Status = DocumentStatus.Finalized;

            var departmentToReceiveDocument = default(long);

            if (virtualDocument.Document.DocumentType == DocumentType.Outgoing)
            {
                departmentToReceiveDocument = GetDestinationDepartmentFromHistory(virtualDocument, token);
            }
            else
            {
                departmentToReceiveDocument = await GetDestinationDepartmentByCodeAsync("registratura", token);
            }

            var newWorkflowResponsible = new WorkflowHistory
            {
                Status = DocumentStatus.Finalized,
                RecipientType = RecipientType.Department.Id,
                RecipientId = departmentToReceiveDocument,
                RecipientName = $"Departamentul {departmentToReceiveDocument}!"
            };
            
            virtualDocument.WorkflowHistory.Add(newWorkflowResponsible);
        }

        private long GetDestinationDepartmentFromHistory(VirtualDocument virtualDocument, CancellationToken token)
        {
            return virtualDocument.WorkflowHistory
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
