using DigitNow.Domain.DocumentManagement.Contracts.Objectives;
using DigitNow.Domain.DocumentManagement.Contracts.Procedures;

namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class Procedure : ExtendedEntity
    {
        public long GeneralObjectiveId { get; set; }
        public long SpecificObjectiveId { get; set; }
        public long ActivityId { get; set; }
        public ProcedureCategory ProcedureCategory { get; set; }
        public long DepartmentId { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public long StateId { get; set; }
        public string Edition { get; set; }
        public string? Revision { get; set; }
        public DateTime StartDate { get; set; }
        public string Scope { get; set; }
        public string DomainOfApplicability { get; set; }
        public string? InternationalReglementation { get; set; }
        public string? PrimaryLegislation { get; set; }
        public string? SecondaryLegislation { get; set; }
        public string? OtherReglementationn { get; set; }
        public string? DefinitionsAndAbbreviations { get; set; }
        public string ProcedureDescription { get; set; }
        public string Responsibility { get; set; }

        public List<ProcedureFunctionary> ProcedureFunctionaries { get; set; }

        public GeneralObjective AssociatedGeneralObjective { get; set; }
        public SpecificObjective AssociatedSpecificObjective { get; set; }
        public Activity AssociatedActivity { get; set; }
    }
}
