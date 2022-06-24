using System.Collections.Generic;

namespace DigitNow.Domain.DocumentManagement.Public.Dashboard.Models
{
    public class UpdateDocumentHeadofDepartmentRequest
    {
        public int DepartmentId { get; set; }
        public List<DocumentInfoRequest> DocumentInfo { get; set; }
    }
}
