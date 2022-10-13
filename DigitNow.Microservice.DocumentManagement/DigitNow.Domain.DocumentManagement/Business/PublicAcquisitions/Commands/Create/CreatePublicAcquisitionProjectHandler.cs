using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.PublicAcquisitions.Commands.Create
{
    public class CreatePublicAcquisitionProjectHandler : ICommandHandler<CreatePublicAcquisitionProjectCommand, ResultObject>
    {
        private readonly IPublicAcquisitionService _publicAcquisitionService;
        private readonly IMapper _mapper;

        public CreatePublicAcquisitionProjectHandler(IPublicAcquisitionService publicAcquisitionService, IMapper mapper)
        {
            _publicAcquisitionService = publicAcquisitionService;
            _mapper = mapper;
        }
        public async Task<ResultObject> Handle(CreatePublicAcquisitionProjectCommand request, CancellationToken cancellationToken)
        {
            var newPublicAcquisition = _mapper.Map<PublicAcquisitionProject>(request);
            await _publicAcquisitionService.AddAsync(newPublicAcquisition, cancellationToken);

            return ResultObject.Created(newPublicAcquisition.Id);
        }
    }
}
