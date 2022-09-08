using DigitNow.Domain.DocumentManagement.Contracts.Objectives;
using DigitNow.Domain.DocumentManagement.Data.Entities.Abstractions;

namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class Objective : ExtendedEntity, IObjective
    {
        public ObjectiveType ObjectiveType { get; set; }
        public string Code { get; set; }
        public ScimState State { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public string ModificationMotive { get; set; }

        #region [ Children ]

        public GeneralObjective GeneralObjective { get; set; }
        public SpecificObjective SpecificObjective { get; set; }

        #endregion
    }
}
