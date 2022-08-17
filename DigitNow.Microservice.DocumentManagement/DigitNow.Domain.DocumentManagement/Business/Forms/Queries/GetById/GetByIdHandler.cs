using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Forms.Queries.GetById
{
    public class GetByIdHandler : IQueryHandler<GetFormByIdQuery, GetFormByIdResponse>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IFormsServices _formsService;

        public GetByIdHandler(
            IMapper mapper,
            IFormsServices formsService,
            DocumentManagementDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _formsService = formsService;
        }

        public async Task<GetFormByIdResponse> Handle(GetFormByIdQuery request, CancellationToken cancellationToken)
        {
            var form = await _dbContext.Forms.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (form == null)
            {
                return null;
            }

            var formFieldMappings = await _formsService.GetFormFieldMappingsByFormIdAsync(request.Id, cancellationToken);

            return null;
        }
    }
}