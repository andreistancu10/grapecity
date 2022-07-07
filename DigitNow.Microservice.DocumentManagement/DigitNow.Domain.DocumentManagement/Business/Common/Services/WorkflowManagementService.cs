using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IWorkflowManagementService
    {
        Task<Document> GetDocumentById(long id, CancellationToken cancellationToken);
        Task CommitChangesAsync(CancellationToken token);
        WorkflowHistory GetLastWorkflowRecord(Document document);
        bool IsTransitionAllowed(WorkflowHistory lastWorkFlowRecord, int[] allowedTransitionStatuses);
        WorkflowHistory GetOldWorkflowResponsible(Document document, Expression<Func<WorkflowHistory, bool>> predicate);

    }
    public class WorkflowManagementService : IWorkflowManagementService
    {
        private readonly IDocumentService _documentService;

        public WorkflowManagementService(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        public async Task<Document> GetDocumentById(long id, CancellationToken cancellationToken)
        {
            var document = await _documentService.FindAsync(x => x.Id == id, cancellationToken);

            switch (document.DocumentType)
            {
                case DocumentType.Incoming:
                    return await _documentService.FindAsync(x => x.Id == id, cancellationToken, doc => doc.IncomingDocument.WorkflowHistory);
                case DocumentType.Internal:
                    return await _documentService.FindAsync(x => x.Id == id, cancellationToken);
                case DocumentType.Outgoing:
                    return await _documentService.FindAsync(x => x.Id == id, cancellationToken, doc => doc.OutgoingDocument.WorkflowHistory);
                default:
                    return document;
            }
        }

        public WorkflowHistory GetLastWorkflowRecord(Document document)
        {
            if (document.IncomingDocument != null)
                return document.IncomingDocument.WorkflowHistory.OrderByDescending(x => x.CreationDate).FirstOrDefault();

            if (document.OutgoingDocument != null)
                return document.OutgoingDocument.WorkflowHistory.OrderByDescending(x => x.CreationDate).FirstOrDefault();

            return null;
        }

        public bool IsTransitionAllowed(WorkflowHistory lastWorkFlowRecord, int[] allowedTransitionStatuses)
        {
            if (lastWorkFlowRecord == null || !allowedTransitionStatuses.Contains((int)lastWorkFlowRecord.Status))
            {
                return false;
            }
            return true;
        }

        public async Task CommitChangesAsync(CancellationToken token)
        {
            await _documentService.CommitChangesAsync(token);
        }

        public WorkflowHistory GetOldWorkflowResponsible(Document document, Expression<Func<WorkflowHistory, bool>> predicate)
        {
            if (document.IncomingDocument != null)
                return ExtractResponsible(document.IncomingDocument.WorkflowHistory.AsQueryable(), predicate);

            if (document.OutgoingDocument != null)
                return ExtractResponsible(document.OutgoingDocument.WorkflowHistory.AsQueryable(), predicate);

            return null;
        }

        private WorkflowHistory ExtractResponsible(IQueryable<WorkflowHistory> history, Expression<Func<WorkflowHistory, bool>> predicate)
        {
            return history.Where(predicate)
                   .OrderByDescending(x => x.CreationDate)
                   .FirstOrDefault();
        }
    }
}
