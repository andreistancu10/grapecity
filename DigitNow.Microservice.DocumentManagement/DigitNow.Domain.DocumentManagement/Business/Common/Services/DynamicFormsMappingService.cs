using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IDynamicFormsMappingService
    {
        Task<List<DynamicFormFillingLogViewModel>> MapToDynamicFormViewModelAsync(IList<DynamicFormFillingLog> dynamicFormFillingLogs, CancellationToken cancellationToken);

    }
    public class DynamicFormsMappingService : IDynamicFormsMappingService
    {
        private readonly IMapper _mapper;
        private readonly DynamicFormRelationsFetcher _dynamicFormRelationsFetcher;


        public DynamicFormsMappingService(IMapper mapper, IServiceProvider serviceProvider)
        {
            _mapper = mapper;
            _dynamicFormRelationsFetcher = new DynamicFormRelationsFetcher(serviceProvider);
        }
        public async Task<List<DynamicFormFillingLogViewModel>> MapToDynamicFormViewModelAsync(IList<DynamicFormFillingLog> dynamicFormFillingLogs, CancellationToken cancellationToken)
        {
            await _dynamicFormRelationsFetcher
                .UseDynamicFormsContext(new DynamicFormsFetcherContext { DynamicFormFillingLogs = dynamicFormFillingLogs })
                .TriggerFetchersAsync(cancellationToken);

            return MapDynamicForms(dynamicFormFillingLogs)
                .ToList();
        }

        private List<DynamicFormFillingLogViewModel> MapDynamicForms(IList<DynamicFormFillingLog> dynamicFormFillingLogs)
        {
            var result = new List<DynamicFormFillingLogViewModel>();

            foreach (var dynamicFormFillingLog in dynamicFormFillingLogs)
            {
                var aggregate = new DynamicFormAggregate
                {
                    DynamicFormFillingLog = dynamicFormFillingLog,
                    Users = _dynamicFormRelationsFetcher.DynamicFormUsers,
                    Departments = _dynamicFormRelationsFetcher.Departments
                };

                result.Add(_mapper.Map<DynamicFormAggregate, DynamicFormFillingLogViewModel>(aggregate));
            }

            return result;
        }
    }
}
