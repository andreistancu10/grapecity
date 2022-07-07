﻿using DigitNow.Domain.DocumentManagement.Data.Entities;
using System;

namespace DigitNow.Domain.DocumentManagement.Business.InternalDocuments.Queries.GetById
{
    public class GetInternalDocumentByIdResponse
    {
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public long CreatedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public long ModifiedBy { get; set; }
        public int DepartmentId { get; set; }
        public int InternalDocumentTypeId { get; set; }
        public int DeadlineDaysNumber { get; set; }
        public string Description { get; set; }
        public string Observation { get; set; }
        public int ReceiverDepartmentId { get; set; }
        public bool? IsUrgent { get; set; }
    }
}