using DigitNow.Domain.DocumentManagement.Data.Entities.Documents.Abstractions;

namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class IncomingDocument : VirtualDocument, IShippable
    {
        public int InputChannelId { get; set; }
        public int IssuerTypeId { get; set; }
        public string IssuerName { get; set; }
        public int ExternalNumber { get; set; }
        public DateTime? ExternalNumberDate { get; set; }
        public string IdentificationNumber { get; set; }
        public string ContentSummary { get; set; }
        public int NumberOfPages { get; set; }
        public int DocumentTypeId { get; set; }
        public string Detail { get; set; }
        public double ResolutionPeriod { get; set; }
        public bool? IsUrgent { get; set; }
        public bool? IsGDPRAgreed { get; set; }

        #region [ References ]

        public List<ConnectedDocument>? ConnectedDocuments { get; set; } = new();
        public ContactDetail ContactDetail { get; set; }
        public DeliveryDetail? DeliveryDetails { get; set; }

        #endregion
    }
}