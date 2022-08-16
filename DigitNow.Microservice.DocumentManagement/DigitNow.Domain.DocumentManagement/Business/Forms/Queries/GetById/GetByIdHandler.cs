using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Forms.Queries.GetById
{
    public class GetByIdHandler : IQueryHandler<GetFormByIdQuery, GetFormByIdResponse>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetByIdHandler(IMapper mapper, DocumentManagementDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<GetFormByIdResponse> Handle(GetFormByIdQuery request, CancellationToken cancellationToken)
        {
            var form = await _dbContext.Forms.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (form == null)
            {
                return null;
            }

            var formFieldMappings = await _dbContext.FormFieldMappings
                .Where(c => c.FormId == form.Id)
                .Include(c => c.FormField)
                .ToListAsync(cancellationToken);



            return null;
        }
    }
}