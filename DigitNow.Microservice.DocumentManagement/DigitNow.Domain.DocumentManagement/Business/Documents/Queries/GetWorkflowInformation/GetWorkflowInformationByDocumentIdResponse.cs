using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;

namespace DigitNow.Domain.DocumentManagement.Business.Documents.Queries.GetWorkflowInformation
{
    public class GetWorkflowInformationByDocumentIdResponse
    {
        public DocumentStatus DocumentStatus { get; set; }

        public UserRole UserRole { get; set; }

    }
}
