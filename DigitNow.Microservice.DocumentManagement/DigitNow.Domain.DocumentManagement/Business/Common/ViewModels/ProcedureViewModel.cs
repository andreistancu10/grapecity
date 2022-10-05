using DigitNow.Domain.DocumentManagement.Contracts.Objectives;
using DigitNow.Domain.DocumentManagement.Contracts.Procedures;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels
{
    public class ProcedureViewModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Edition { get; set; }
        public string Revision { get; set; }
        public BasicViewModel CreatedBy { get; set; }
        public DateTime StartDate { get; set; }
        public ProcedureCategory ProcedureCategory { get; set; }
        public string Code { get; set; }
        public long StateId { get; set; }
        public BasicViewModel SpecificObjective { get; set; }
        public BasicViewModel Department { get; set; }

    }
}
