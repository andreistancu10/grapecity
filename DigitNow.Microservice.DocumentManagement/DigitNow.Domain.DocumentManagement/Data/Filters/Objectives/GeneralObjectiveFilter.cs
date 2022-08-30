using DigitNow.Domain.DocumentManagement.Contracts.Objectives;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.Objectives
{
    public class GeneralObjectiveFilter : DataFilter
    {
        public GeneralObjectiveCreationDateFilter GeneralObjectiveRegistrationDateFilter { get; set; }
        public GeneralObjectiveTitleFilter GeneralObjectiveTitleFilter { get; set; }
        public GeneralObjectiveCodeFilter GeneralObjectiveCodeFilter { get; set; }
        public GeneralObjectiveStateFilter GeneralObjectiveStateFilter { get; set; }

        public static GeneralObjectiveFilter Empty => new GeneralObjectiveFilter();
    }

    public class GeneralObjectiveCreationDateFilter
    {
        public DateTime CreationDate { get; set; }
    }

    public class GeneralObjectiveTitleFilter
    {
        public string Title { get; set; }
    }

    public class GeneralObjectiveCodeFilter
    {
        public string Code { get; set; }
    }

    public class GeneralObjectiveStateFilter
    {
        public ObjectiveState State { get; set; }
    }
}
