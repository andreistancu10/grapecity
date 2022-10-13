using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.PublicAcquisitions.Queries.GetById
{
    public class GetPublicAcquisitionProjectByIdQuery : IQuery<GetPublicAcquisitionProjectViewModel>
    {
        public long Id { get; set; }
        public GetPublicAcquisitionProjectByIdQuery(long id)
        {
            Id = id;
        }
    }
}
