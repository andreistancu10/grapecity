using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using System;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels
{
    public class DocumentStatusViewModel
    {
        public DocumentStatus Status { get; set; }
        public string Label { get; set; }
    }
}
