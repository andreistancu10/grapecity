namespace DigitNow.Domain.DocumentManagement.Business.Standards.Queries.GetById
{
    public class GetStandardByIdResponse
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public long StateId { get; set; }
        public string Activity { get; set; }
        public long DepartmentId { get; set; }
        public List<StandardFunctionaryResponse> StandardFunctionaries { get; set; }
        public DateTime Deadline { get; set; }
        public string Observations { get; set; }
        public string ModificationMotive { get; set; }
    }

    public class StandardFunctionaryResponse
    {
        public long Id { get; set; }
        public long StandardId { get; set; }
        public long FunctionaryId { get; set; }
    }
}
