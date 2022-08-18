using DigitNow.Domain.DocumentManagement.Public.Forms.Models;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Forms.Commands
{
    public class SaveFormDataCommand : ICommand<ResultObject>
    {
        public long FormId { get; set; }
        public List<BasicRequestModel> Values { get; set; }
    }
}