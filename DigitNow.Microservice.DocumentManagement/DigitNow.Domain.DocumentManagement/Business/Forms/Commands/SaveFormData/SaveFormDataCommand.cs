using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Forms.Commands
{
    public class SaveFormDataCommand : ICommand<ResultObject>
    {
        public long FormId { get; set; }
        public List<BasicValueViewModel> Values { get; set; }
    }
}