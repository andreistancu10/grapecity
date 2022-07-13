﻿using System;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Models
{
    public class DocumentViewModel
    {
        public long Id { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int RegistrationNumber { get; set; }
        public string IssuerName { get; set; }
        public string Recipient { get; set; }
        public int DocumentType { get; set; }
        public string DocumentCategory { get; set; }
        public int ResolutionDuration { get; set; }
        public int Status { get; set; }
        public string DispatchBy { get; set; }
        public string User { get; set; }
    }
}
