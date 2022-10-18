using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IStandardMappingService
    {
        Task<List<StandardViewModel>> MapToStandardViewModelAsync(IList<Standard> standards, CancellationToken cancellationToken);
    }
    public class StandardMappingService: IStandardMappingService
    {
        private readonly IMapper _mapper;
        private readonly StandardRelationsFetcher _standardRelationsFetcher;

        public StandardMappingService(IMapper mapper, IServiceProvider serviceProvider)
        {
            _mapper = mapper;
            _standardRelationsFetcher = new StandardRelationsFetcher(serviceProvider);
        }
        public async Task<List<StandardViewModel>> MapToStandardViewModelAsync(IList<Standard> standards, CancellationToken cancellationToken)
        {
            await _standardRelationsFetcher
                .UseStandardFetcherContext()
               .TriggerFetchersAsync(cancellationToken);

            return MapStandards(standards).ToList();
        }
        private List<StandardViewModel> MapStandards(IList<Standard> standards)
        {
            var result = new List<StandardViewModel>();

            foreach (var standard in standards)
            {
                var aggregate = new StandardAggregate
                {
                    Standard = standard,
                    Departments = _standardRelationsFetcher.Departments
                };

                result.Add(_mapper.Map<StandardAggregate, StandardViewModel>(aggregate));
            }

            return result;
        }
    }
}
