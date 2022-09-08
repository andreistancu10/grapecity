using AutoMapper;
using DigitNow.Adapters.MS.Catalog;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
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
        private readonly IMailSenderService _mailSenderService;
        public CreateDocumentDeliveryDetailsHandler(
            DocumentManagementDbContext dbContext, 
            IMapper mapper, 
            ICatalogAdapterClient catalogAdapterClient,
            IIdentityService identityService,
            IMailSenderService mailSenderService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _catalogAdapterClient = catalogAdapterClient;
            _identityService = identityService;
            _mailSenderService = mailSenderService;
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

            if(deliveryDetails.DeliveryMode == (int)TransmissionMode.Post || deliveryDetails.DeliveryMode == (int)TransmissionMode.Widnow)
            {
                await _mailSenderService.SendMail_OnReplySent(document, cancellationToken);
            }

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

            document.Status = DocumentStatus.Finalized;

            //ToDo: clarification needed
            //if (deliveryDetails.DeliveryMode == (int)TransmissionMode.DirectTransmission && document.DocumentType == DocumentType.Outgoing)
            //{
            //    return;
            //}

            var newWorkflowResponsible = new WorkflowHistoryLog
            {
                DocumentId = document.Id,
                DocumentStatus = DocumentStatus.Finalized,
                RecipientType = RecipientType.Department.Id,
                RecipientId = departmentToReceiveDocument.Id,
                RecipientName = $"Departamentul {departmentToReceiveDocument.Name}"
            };
            
            await _dbContext.WorkflowHistoryLogs.AddAsync(newWorkflowResponsible, token);

            //Make visible to registry after delivery
            await _dbContext.DocumentActions.AddAsync(new DocumentAction
            {
                DocumentId = document.Id,
                Action = UserActionsOnDocument.ShipsDocument,
                DepartmentId = departmentToReceiveDocument.Id,
                ResposibleId = default
            }, token);
        }
    }
}
