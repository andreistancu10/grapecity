using HTSS.Platform.Core.Domain;
using System;

namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class WorkflowHistory : ExtendedEntity
    {
        public int RecipientType { get; set; }
        public long RecipientId { get; set; }
        public string RecipientName { get; set; }
        public string Remarks { get; set; }
        public string DeclineReason { get; set; }
        public string Resolution { get; set; }
        public DateTime? OpinionRequestedUntil { get; set; }
        public int Status { get; set; }
        public int RegistrationNumber { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
