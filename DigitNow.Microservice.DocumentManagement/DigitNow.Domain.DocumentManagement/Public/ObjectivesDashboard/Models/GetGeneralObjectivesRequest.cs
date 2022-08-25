using DigitNow.Domain.DocumentManagement.Contracts.Objectives;

namespace DigitNow.Domain.DocumentManagement.Public.ObjectivesDashboard.Models
{
    public class GetGeneralObjectivesRequest
    {
        public int Page { get; set; } = 1;
        public int Count { get; set; } = 10;
        public GeneralObjectiveFilterDto Filter { get; set; }
    }

    public class GeneralObjectiveFilterDto
    {
        public ObjectiveCreationDateFilterDto CreationDateFilter { get; set; }
        public ObjectiveTitleFilterDto ObjectiveTitleFilter { get; set; }
        public ObjectiveCodeFilterDto ObjectiveCodeFilter { get; set; }
        public ObjectiveStateFilterDto ObjectiveStateFilter { get; set; }
    }

    public class ObjectiveCreationDateFilterDto
    {
        public DateTime CreationDate { get; set; }
    }
    public  class ObjectiveTitleFilterDto
    {
        public string Title { get; set; }   
    }
    public class ObjectiveCodeFilterDto
    {
        public string Code { get; set; }    
    }
    public class ObjectiveStateFilterDto
    {
        public ObjectiveState State { get; set; }
    }
}
