using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Risks.Queries.GetById
{
    public class GetRiskByIdQuery : IQuery<GetRiskViewModel>
    {
        public long Id { get; set; }
        public GetRiskByIdQuery(long id)
        {
            Id = id;
        }
    }
}
