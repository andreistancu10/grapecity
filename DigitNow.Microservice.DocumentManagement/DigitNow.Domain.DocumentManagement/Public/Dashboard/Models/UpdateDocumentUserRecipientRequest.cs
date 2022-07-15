using System.Collections.Generic;

namespace DigitNow.Domain.DocumentManagement.Public.Dashboard.Models
{
    public class UpdateDocumentUserRecipientRequest
    {
        public int UserId { get; set; }
        public List<long> RegistrationNumbers { get; set; }
    }
}
