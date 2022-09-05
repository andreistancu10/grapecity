using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.DynamicForms.Queries.Filter
{
    public class FilterDynamicFormsHandler : IQueryHandler<DynamicFilterFormsQuery, IEnumerable<DynamicFilterFormsResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IDynamicFormsService _dynamicFormsService;

        public FilterDynamicFormsHandler(
            IMapper mapper,
            IDynamicFormsService dynamicFormsService)
        {
            _mapper = mapper;
            _dynamicFormsService = dynamicFormsService;
        }

        public async Task<IEnumerable<DynamicFilterFormsResponse>> Handle(DynamicFilterFormsQuery request, CancellationToken cancellationToken)
        {
            var forms = await _dynamicFormsService.GetDynamicFormsQueryAsync().ToListAsync(cancellationToken);

            return forms.Select(c => _mapper.Map<DynamicFilterFormsResponse>(c));
        }
    }
}