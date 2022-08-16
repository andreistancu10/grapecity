using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Forms.Queries.Filter
{
    public class FilterFormsHandler : IQueryHandler<FilterFormsQuery, IEnumerable<FilterFormsResponse>>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IMapper _mapper;

        public FilterFormsHandler(IMapper mapper, DocumentManagementDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<FilterFormsResponse>> Handle(FilterFormsQuery request, CancellationToken cancellationToken)
        {
            var forms = await _dbContext.Forms.ToListAsync(cancellationToken);

            return forms.Select(c => _mapper.Map<FilterFormsResponse>(c));
        }
    }
}