﻿using System;

namespace DigitNow.Domain.DocumentManagement.Business.Dashboard.Queries
{
    public class GetDocumentResponse
    {
        public long DocumentId { get; set; } 
        public long VirtualDocumentId { get; set; }
        public string DocumentType { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int RegistrationNumber { get; set; }
        public string IssuerName { get; set; }
        public string RecipientName { get; set; }        
        public int DocumentCategory { get; set; }
        public double ResolutionPeriod { get; set; }
        public int Status { get; set; }
        public string DispatchedBy { get; set; }
        public string User { get; set; }
        public bool IsDispatched { get; set; }
    }
}