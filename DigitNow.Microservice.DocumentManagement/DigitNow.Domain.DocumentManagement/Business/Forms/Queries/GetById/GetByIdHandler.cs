using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Forms.Queries.GetById
{
    public class GetByIdHandler : IQueryHandler<GetFormByIdQuery, List<FormControlViewModel>>
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

        public async Task<List<FormControlViewModel>> Handle(GetFormByIdQuery request, CancellationToken cancellationToken)
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

            var formFields = formFieldMappings.Select(c => c.FormField).ToList();

            //TODO: determine whether to use Fetchers or leave it as it is. NOTE: Fetchers code already written.
            var viewModels = new List<FormControlViewModel>();

            foreach (var mapping in formFieldMappings)
            {
               var viewModel= _mapper.Map<FormControlViewModel>(new FormControlAggregate
                {
                    FormFields = formFields,
                    FormFieldMapping = mapping
                });

               viewModels.Add(viewModel);
            }

            return viewModels;
        }
    }
}