using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.InternalDocuments.Queries.GetById;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Infrastructure.Data.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Forms.Queries
{
    public class FilterFormsHandler : IQueryHandler<FilterFormsQuery, ResultPagedList<FilterFormsResponse>>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IMapper _mapper;

        public FilterFormsHandler(IMapper mapper, DocumentManagementDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }
        
        public async Task<ResultPagedList<FilterFormsResponse>> Handle(FilterFormsQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}