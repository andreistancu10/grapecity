using DigitNow.Domain.DocumentManagement.Public.Dashboard.Models;
using HTSS.Platform.Core.CQRS;
using System.Collections.Generic;

namespace DigitNow.Domain.DocumentManagement.Business.Dashboard.Commands.Update
{
    public class UpdateDepartmentForDocumentCommand : ICommand<ResultObject>
    {
        public int DepartmentId { get; set; }
        public List<DocumentInfoRequest> DocumentInfo { get; set; }
    }
}
