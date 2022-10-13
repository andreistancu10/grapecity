using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.PublicAcquisitions.Queries.GetById
{
    public class GetPublicAcquisitionByIdHandler : IQueryHandler<GetPublicAcquisitionProjectByIdQuery, GetPublicAcquisitionProjectViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IPublicAcquisitionService _publicAcquisitionService;

        public GetPublicAcquisitionByIdHandler(IMapper mapper, IPublicAcquisitionService publicAcquisitionService)
        {
            _mapper = mapper;
            _publicAcquisitionService = publicAcquisitionService;
        }

        public async Task<GetPublicAcquisitionProjectViewModel> Handle(GetPublicAcquisitionProjectByIdQuery request, CancellationToken cancellationToken)
        {
            var publicAcquisitionFromDb = await _publicAcquisitionService.GetByIdQuery(request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (publicAcquisitionFromDb == null) return new GetPublicAcquisitionProjectViewModel();

            return _mapper.Map<GetPublicAcquisitionProjectViewModel>(publicAcquisitionFromDb);
        }
    }
}
