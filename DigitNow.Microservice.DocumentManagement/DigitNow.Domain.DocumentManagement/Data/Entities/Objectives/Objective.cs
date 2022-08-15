using DigitNow.Domain.DocumentManagement.Contracts.Objectives;
using DigitNow.Domain.DocumentManagement.Data.Entities.Objectives.Abstractions;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.Objectives
{
    public class Objective : ExtendedEntity, IObjective
    {
        public ObjectiveType ObjectiveType { get; set; }
        public string Code { get; set; }
        public ObjectiveState State { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }

        #region [ Children ]
        public GeneralObjective GeneralObjective { get; set; }
        public SpecificObjective SpecificObjective { get; set; }
        #endregion

        public List<ObjectiveUploadedFile> ObjectiveUploadedFiles { get; set; }
    }
}
