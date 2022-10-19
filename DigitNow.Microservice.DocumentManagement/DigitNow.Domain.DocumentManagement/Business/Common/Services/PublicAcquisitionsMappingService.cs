using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IPublicAcquisitionsMappingService
    {
        Task<List<PublicAcquisitionViewModel>> MapToPublicAcquisitionViewModelAsync(IList<PublicAcquisitionProject> publicAcquisitionProjects, CancellationToken cancellationToken);
    }
    public class PublicAcquisitionsMappingService : IPublicAcquisitionsMappingService
    {
        private readonly IMapper _mapper;
        private readonly PublicAcquisitionRelationsFetcher _publicAcquisitionRelationsFetcher;

        public PublicAcquisitionsMappingService(IMapper mapper, IServiceProvider serviceProvider)
        {
            _mapper = mapper;
            _publicAcquisitionRelationsFetcher = new PublicAcquisitionRelationsFetcher(serviceProvider);
        }

        public async Task<List<PublicAcquisitionViewModel>> MapToPublicAcquisitionViewModelAsync(IList<PublicAcquisitionProject> publicAcquisitionProjects, CancellationToken cancellationToken)
        {
            await _publicAcquisitionRelationsFetcher
                .UsePublicAcquisitionsContext(new PublicAcquisitionProjectFetcherContext { PublicAcquisitionProjects = publicAcquisitionProjects })
                .TriggerFetchersAsync(cancellationToken);

            return MapPublicAcquisitionProjects(publicAcquisitionProjects).ToList();
        }

        private List<PublicAcquisitionViewModel> MapPublicAcquisitionProjects(IList<PublicAcquisitionProject> publicAcquisitionProjects)
        {
            var result = new List<PublicAcquisitionViewModel>();

            foreach (var project in publicAcquisitionProjects)
            {
                var aggregate = new PublicAcquisitionAggregate
                {
                    PublicAcquisitionProject = project,
                    EstablishedProcedures = _publicAcquisitionRelationsFetcher.EstablishedProcedures,
                    CpvCodes = _publicAcquisitionRelationsFetcher.CpvCodes
                };

                result.Add(_mapper.Map<PublicAcquisitionAggregate, PublicAcquisitionViewModel>(aggregate));
            }

            return result;
        }
    }
}
