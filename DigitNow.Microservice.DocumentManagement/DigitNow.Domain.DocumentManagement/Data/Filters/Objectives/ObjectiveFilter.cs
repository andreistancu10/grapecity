using DigitNow.Domain.DocumentManagement.Contracts.Objectives;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.Objectives
{
    public class ObjectiveFilter : DataFilter
    {
        public ObjectiveCreationDateFilter ObjectiveRegistrationDateFilter { get; set; }
        public ObjectiveTitleFilter ObjectiveTitleFilter { get; set; }
        public ObjectiveCodeFilter ObjectiveCodeFilter { get; set; }
        public ObjectiveStateFilter ObjectiveStateFilter { get; set; }

        public static ObjectiveFilter Empty => new ObjectiveFilter();
    }

    public class ObjectiveCreationDateFilter
    {
        public DateTime CreationDate { get; set; }
    }

    public class ObjectiveTitleFilter
    {
        public string Title { get; set; }
    }

    public class ObjectiveCodeFilter
    {
        public string Code { get; set; }
    }

    public class ObjectiveStateFilter
    {
        public ObjectiveState State { get; set; }
    }
}
