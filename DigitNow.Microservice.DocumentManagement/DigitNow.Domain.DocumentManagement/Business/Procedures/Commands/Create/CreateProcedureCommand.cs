using DigitNow.Domain.DocumentManagement.Contracts.Procedures;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Procedures.Commands.Create
{
    public class CreateProcedureCommand : ICommand<ResultObject>
    {
        public long GeneralObjectiveId { get; set; }
        public long SpecificObjectiveId { get; set; }
        public long ActivityId { get; set; }
        public ProcedureCategory ProcedureCategory { get; set; }
        public long DepartmentId { get; set; }
        public string Title { get; set; }
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

        public List<long> ProcedureFunctionariesIds { get; set; }
        public List<long> UploadedFileIds { get; set; }
    }
}
