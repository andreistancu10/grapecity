using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.DynamicForms.Queries.Filter
{
    public class FilterDynamicFormsHandler : IQueryHandler<DynamicFilterFormsQuery, IEnumerable<DynamicFilterFormsResponse>>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IMapper _mapper;

        public FilterDynamicFormsHandler(IMapper mapper, DocumentManagementDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<DynamicFilterFormsResponse>> Handle(DynamicFilterFormsQuery request, CancellationToken cancellationToken)
        {
            var forms = await _dbContext.DynamicForms.AsNoTracking().ToListAsync(cancellationToken);

            return forms.Select(c => _mapper.Map<DynamicFilterFormsResponse>(c));
        }
    }
}