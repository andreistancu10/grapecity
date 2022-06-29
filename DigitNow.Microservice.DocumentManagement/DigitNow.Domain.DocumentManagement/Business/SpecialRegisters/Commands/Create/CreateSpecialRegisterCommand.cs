using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.SpecialRegisters.Commands.Create
{
    public class CreateSpecialRegisterCommand : ICommand<ResultObject>
    {
        public int DocumentCategoryId { get; set; }
        public string  Name { get; set; }
        public string Observations { get; set; }
    }
}
