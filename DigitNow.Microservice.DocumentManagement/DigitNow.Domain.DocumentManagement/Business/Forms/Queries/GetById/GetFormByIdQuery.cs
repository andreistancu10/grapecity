using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Forms.Queries.GetById
{
    public class GetFormByIdQuery :  IQuery<List<FormControlViewModel>>
    {
        public long Id { get; set; }
    }
}