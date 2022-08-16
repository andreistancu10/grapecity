using System.ComponentModel.DataAnnotations.Schema;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.Objectives
{
    public class SpecificObjective : VirtualObjective
    {
        public long DepartmentId { get; set; }
        public long GeneralObjectiveId { get; set; }

        #region [ References ]

        public GeneralObjective AssociatedGeneralObjective { get; set; }
        public List<SpecificObjectiveFunctionary> SpecificObjectiveFunctionarys { get; set; }
   
        #endregion

    }
}
