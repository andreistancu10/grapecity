using DigitNow.Domain.DocumentManagement.Contracts.Objectives;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.Abstractions
{
    public interface IObjective
    {
        public string Code { get; set; }
        public long StateId{ get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public string ModificationMotive { get; set; }
    }
}
