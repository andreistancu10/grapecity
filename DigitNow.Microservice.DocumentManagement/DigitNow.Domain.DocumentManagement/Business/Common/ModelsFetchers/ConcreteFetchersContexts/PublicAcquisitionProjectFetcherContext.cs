using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts
{
    internal class PublicAcquisitionProjectFetcherContext : ModelFetcherContext
    {
        public IList<PublicAcquisitionProject> PublicAcquisitionProjects
        {
            get => this[nameof(PublicAcquisitionProject)] as IList<PublicAcquisitionProject>;
            set => this[nameof(PublicAcquisitionProject)] = value;
        }
    }
}
