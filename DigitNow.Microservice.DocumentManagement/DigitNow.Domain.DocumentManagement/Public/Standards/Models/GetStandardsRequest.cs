namespace DigitNow.Domain.DocumentManagement.Public.Standards.Models
{
    public class GetStandardsRequest
    {
        public int Page { get; set; } = 1;
        public int Count { get; set; } = 10;
        public StandardFilterDto Filter { get; set; }
    }
    public class StandardFilterDto
    {
        public TitleFilterDto TitleFilter { get; set; }
        public DepartmentFilterDto DepartmentFilter { get; set; }
        public FunctionariesFilterDto FunctionariesFilter { get; set; }
        public DeadlineFilterDto DeadlineFilter { get; set; }
    }

    public class TitleFilterDto
    {
        public string Title { get; set; }
    }
    public class DepartmentFilterDto
    {
        public string DepartmentId { get; set; }
    }
    public class FunctionariesFilterDto
    {
        public List<long> FunctionariesIds { get; set; }
    }
    public class DeadlineFilterDto
    {
        public DateTime Deadline { get; set; }
    }
}
