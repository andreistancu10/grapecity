using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels
{
    public class GetPerformanceIndicatorViewModel
    {
        public long GeneralObjectiveId { get; set; }
        public long SpecificObjectiveId { get; set; }
        public long ActivityId { get; set; }
        public long DepartmentId { get; set; }
        public long StateId { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Target { get; set; }
        public string QuantificationFormula { get; set; }
        public string ResultIndicator { get; set; }
        public DateTime Deadline { get; set; }
        public string SolutionStage { get; set; }
        public string Observations { get; set; }
        public string ModificationMotive { get; set; }
        public List<long> FunctionaryIds { get; set; }
    }
}
