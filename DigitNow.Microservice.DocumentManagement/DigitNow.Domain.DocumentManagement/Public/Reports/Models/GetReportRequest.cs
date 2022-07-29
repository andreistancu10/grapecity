using System;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data.Filters.ConcreteFilters;

namespace DigitNow.Domain.DocumentManagement.Public.Reports.Models
{
    public class GetExpiredReportRequest
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    } 
    
    public class GetToExpireReportRequest
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}