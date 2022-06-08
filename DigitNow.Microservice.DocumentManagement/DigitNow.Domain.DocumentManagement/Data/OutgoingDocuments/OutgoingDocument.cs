using System;
using System.Collections.Generic;
using HTSS.Platform.Core.Domain;

namespace DigitNow.Domain.DocumentManagement.Data.OutgoingDocuments
{
    public class OutgoingDocument : Entity
    {
        public OutgoingDocument(long id)
        {
            Id = id;
        }
        public string DenumireAdresant { get; set; }
        public string AdresaAdresant { get; set; }
        public string UniqueIdentityNumber { get; set; }
        public string Country { get; set; }
        public string County { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string StreetNo { get; set; }
        public string Building { get; set; }
        public string BuildingEntranceNo { get; set; }
        public string Floor { get; set; }
        public string ApartmentNo { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public int PageCount { get; set; }
        public string ContentDescription { get; set; }
        public string Recipient { get; set; }
        public string DocumentType { get; set; }
        public string DocumentTypeDetails { get; set; }
        public string SettlementDuration { get; set; }
        public DateTime ResponseDate { get; set; }
        public string AssociatedWith { get; set; }
        public string No { get; set; }
        public DateTime Date { get; set; }
        public bool IsUrgent { get; set; }
        public string Registrar { get; set; }
        public List<OutgoingDocument> RelatedOutboundDocuments { get; set; }
    }
}
