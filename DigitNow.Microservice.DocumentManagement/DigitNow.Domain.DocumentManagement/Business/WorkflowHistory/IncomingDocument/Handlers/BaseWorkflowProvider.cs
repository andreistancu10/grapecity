using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.IncomingDocument.Handlers
{
    public class BaseWorkflowProvider
    {
        public static IDocumentService DocumentService { get; set; }
        public static IIdentityService IdentityService { get; set; }
        public Document Document { get; set; }

        public async Task<Document> GetDocumentById(long id, CancellationToken token)
        {
            var document = await DocumentService.FindAsync(doc => doc.Id == id, token, x => x.IncomingDocument.WorkflowHistory);
            return document;
        }

        public bool ValidateTransitionStatus(Data.Entities.WorkflowHistory record, int[] allowedStatusesForTransition)
        {
            if (record == null || !allowedStatusesForTransition.Contains(record.Status))
            {
                return false;
            }
            return true;
        }
    }
}
