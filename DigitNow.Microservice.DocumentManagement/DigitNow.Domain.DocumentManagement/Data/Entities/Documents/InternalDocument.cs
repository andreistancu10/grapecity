
namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class InternalDocument : VirtualDocument
    {
        public int SourceDepartmentId { get; set; }
        public int InternalDocumentTypeId { get; set; }
        public int DeadlineDaysNumber { get; set; }
        public string Description { get; set; }
        public string Observation { get; set; }
        public bool? IsUrgent { get; set; }
    }
}