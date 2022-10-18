namespace DigitNow.Domain.DocumentManagement.Data.Filters.Standards
{
    public class StandardFilter : DataFilter
    {
        public TitleFilter TitleFilter { get; set; }
        public DepartmentFilter DepartmentFilter { get; set; }
        public FunctionariesFilter FunctionariesFilter { get; set; }
        public DeadlineFilter DeadlineFilter { get; set; }

        public static StandardFilter Empty => new();
    }
    public class TitleFilter
    {
        public string Title { get; set; }
    }
    public class DepartmentFilter
    {
        public long DepartmentId { get; set; }
    }
    public class FunctionariesFilter
    {
        public List<long> FunctionariesIds { get; set; }
    }
    public class DeadlineFilter
    {
        public DateTime Deadline { get; set; }
    }
}
