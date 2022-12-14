using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.SpecialRegisters.Queries.Exports
{
    public class SpecialRegisterExportExcelHandler : IQueryHandler<SpecialRegisterExportExcelQuery, List<SpecialRegisterExportViewModel>>
    {
        private readonly IMapper _mapper;
        private readonly ISpecialRegisterService _specialRegisterService;
        private readonly SpecialRegisterRelationsFetcher _specialRegisterRelationsFetcher;

        public SpecialRegisterExportExcelHandler(
            IMapper mapper,
            ISpecialRegisterService specialRegisterService,
            IServiceProvider serviceProvider)
        {
            _mapper = mapper;
            _specialRegisterService = specialRegisterService;
            _specialRegisterRelationsFetcher = new SpecialRegisterRelationsFetcher(serviceProvider);
        }

        public async Task<List<SpecialRegisterExportViewModel>> Handle(SpecialRegisterExportExcelQuery request, CancellationToken cancellationToken)
        {
            var registers = await _specialRegisterService.FindAllAsync(cancellationToken);
            var result = new List<SpecialRegisterExportViewModel>();

            await _specialRegisterRelationsFetcher                
                .TriggerFetchersAsync(cancellationToken);

            foreach (var specialRegister in registers)
            {
                var aggregate = new SpecialRegisterViewModelAggregate
                {
                    SpecialRegister = specialRegister,
                    Categories = _specialRegisterRelationsFetcher.DocumentCategories
                };

                var specialRegisterViewModel = _mapper.Map<SpecialRegisterViewModel>(aggregate);
                var specialRegisterExportViewModel = _mapper.Map<SpecialRegisterExportViewModel>(specialRegisterViewModel);
                result.Add(specialRegisterExportViewModel);
            }

            return result;
        }
    }
}