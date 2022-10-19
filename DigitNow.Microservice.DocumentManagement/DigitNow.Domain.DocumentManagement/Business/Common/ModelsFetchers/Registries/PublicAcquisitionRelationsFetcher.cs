using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.GenericFetchers;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries
{
    internal class PublicAcquisitionRelationsFetcher : BaseRelationsFetcher
    {
        public IReadOnlyList<PublicAcquisitionProject> PublicAcquisitionProjects
            => GetItems<PublicAcquisitionProjectFetcher, PublicAcquisitionProject>();

        public IReadOnlyList<EstablishedProcedureModel> EstablishedProcedures
            => GetItems<GenericEstablishedProceduresFetcher, EstablishedProcedureModel>();

        public IReadOnlyList<CpvCodeModel> CpvCodes
            => GetItems<GenericCpvCodesFetcher, CpvCodeModel>();

        public PublicAcquisitionRelationsFetcher(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Aggregator
                .UseGenericRemoteFetcher<GenericCpvCodesFetcher>()
                .UseGenericRemoteFetcher<GenericEstablishedProceduresFetcher>();
        }

        public PublicAcquisitionRelationsFetcher UsePublicAcquisitionsContext(PublicAcquisitionProjectFetcherContext context)
        {
            Aggregator
               .UseInternalFetcher<PublicAcquisitionProjectFetcher>(context);

            return this;
        }
    }
}
