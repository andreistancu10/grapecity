using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.ProcedureHistories.Queries.GetById
{
    public class GetProcedureHistoryByIdQuery : IQuery<ProcedureHistoryViewModel>
    {
        public long Id { get; set; }

        public GetProcedureHistoryByIdQuery(long id)
        {
            Id = id;
        }
    }
}
