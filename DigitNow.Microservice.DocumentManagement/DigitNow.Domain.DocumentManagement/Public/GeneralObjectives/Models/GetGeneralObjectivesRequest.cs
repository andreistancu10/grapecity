using DigitNow.Domain.DocumentManagement.Contracts.Objectives;

namespace DigitNow.Domain.DocumentManagement.Public.GeneralObjectives.Models
{
    public class GetGeneralObjectivesRequest
    {
        public int Page { get; set; } = 1;
        public int Count { get; set; } = 10;
        public GeneralObjectiveFilterDto Filter { get; set; }
    }

    public class GeneralObjectiveFilterDto
    {
        public ObjectiveRegistrationDateFilterDto ObjectiveRegistrationDateFilter { get; set; }
        public ObjectiveTitleFilterDto ObjectiveTitleFilter { get; set; }
        public ObjectiveCodeFilterDto ObjectiveCodeFilter { get; set; }
        public ObjectiveStateFilterDto ObjectiveStateFilter { get; set; }
    }

    public class ObjectiveRegistrationDateFilterDto
    {
        public DateTime ObjectiveRegistrationDate { get; set; }
    }
    public  class ObjectiveTitleFilterDto
    {
        public string ObjectiveTitle { get; set; }   
    }
    public class ObjectiveCodeFilterDto
    {
        public string ObjectiveCode { get; set; }    
    }
    public class ObjectiveStateFilterDto
    {
        public ObjectiveState ObjectiveState { get; set; }
    }
}
