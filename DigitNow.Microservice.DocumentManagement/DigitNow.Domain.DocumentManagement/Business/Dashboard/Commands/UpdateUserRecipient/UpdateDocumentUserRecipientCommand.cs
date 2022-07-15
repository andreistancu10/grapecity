using HTSS.Platform.Core.CQRS;
using System.Collections.Generic;

namespace DigitNow.Domain.DocumentManagement.Business.Dashboard.Commands.UpdateUserRecipient
{
    public class UpdateDocumentUserRecipientCommand : ICommand<ResultObject>
    {
        public int UserId { get; set; }
        public List<long> DocumentIds { get; set; }
    }
}
