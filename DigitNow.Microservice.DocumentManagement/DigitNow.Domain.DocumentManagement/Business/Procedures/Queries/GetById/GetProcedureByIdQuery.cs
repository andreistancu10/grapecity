using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Procedures.Queries.GetById
{
    public class GetProcedureByIdQuery : IQuery<GetProcedureViewModel>
    {
        public long Id { get; set; }
        public GetProcedureByIdQuery(long id)
        {
            Id = id;
        }
    }
}
