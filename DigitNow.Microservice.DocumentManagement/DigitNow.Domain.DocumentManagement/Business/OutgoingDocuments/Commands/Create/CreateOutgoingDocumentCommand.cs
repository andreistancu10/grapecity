using HTSS.Platform.Core.CQRS;
using System;
using System.Collections.Generic;

namespace DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Commands.Create
{
    public class CreateOutgoingDocumentCommand : ICommand<ResultObject>
    {
        public DateTime? RegistrationDate { get; set; }
        public string RegistrationNumber { get; set; }
        public int InputChannelId { get; set; }
        public int IssuerTypeId { get; set; }
        public string IssuerName { get; set; }
        public string IdentificationNumber { get; set; }
        public int ExternalNumber { get; set; }
        public DateTime? ExternalNumberDate { get; set; }
        public string ContentSummary { get; set; }
        public int NumberOfPages { get; set; }
        public int RecipientId { get; set; }
        public int DocumentTypeId { get; set; }
        public string Detail { get; set; }
        public double ResolutionPeriod { get; set; }
        public bool? IsUrgent { get; set; }
        public bool? IsGDPRAgreed { get; set; }
        public string User { get; set; }
        public List<string> ConnectedDocumentIds { get; set; }
    }
}
