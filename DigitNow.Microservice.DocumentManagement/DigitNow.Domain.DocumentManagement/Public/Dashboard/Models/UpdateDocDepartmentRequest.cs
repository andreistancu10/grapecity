
using System.Collections.Generic;

namespace DigitNow.Domain.DocumentManagement.Public.Dashboard.Models
{
    public class UpdateDocDepartmentRequest
    {
        public int DepartmentId { get; set; }
        public List<DocumentInfo> DocumentInfo { get; set; }
    }
}
