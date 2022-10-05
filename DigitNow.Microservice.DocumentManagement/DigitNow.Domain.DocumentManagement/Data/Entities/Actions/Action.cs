using DigitNow.Domain.DocumentManagement.Contracts.Objectives;
using DigitNow.Domain.DocumentManagement.Data.Entities.Actions;

namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class Action : ExtendedEntity
    {
        public long ActivityId { get; set; }
        public long DepartmentId { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public long StateId{ get; set; }
        public string Details { get; set; }
        public string ModificationMotive { get; set; }


        #region [ References ]
        public Activity AssociatedActivity { get; set; }
        public List<ActionFunctionary> ActionFunctionaries { get; set; }
        public List<Risk> Risks { get; set; }
        #endregion

    }
}
