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
        public GeneralObjectiveRegistrationDateFilterDto GeneralObjectiveRegistrationDateFilter { get; set; }
        public GeneralObjectiveTitleFilterDto GeneralObjectiveTitleFilter { get; set; }
        public GeneralObjectiveCodeFilterDto GeneralObjectiveCodeFilter { get; set; }
        public GeneralObjectiveStateFilterDto GeneralObjectiveStateFilter { get; set; }
    }

    public class GeneralObjectiveRegistrationDateFilterDto
    {
        public DateTime GeneralObjectiveRegistrationDate { get; set; }
    }
    public class GeneralObjectiveTitleFilterDto
    {
        public string GeneralObjectiveTitle { get; set; }   
    }
    public class GeneralObjectiveCodeFilterDto
    {
        public string GeneralObjectiveCode { get; set; }    
    }
    public class GeneralObjectiveStateFilterDto
    {
        public ScimState GeneralObjectiveState { get; set; }
    }
}
