﻿using DigitNow.Domain.DocumentManagement.Contracts.Objectives;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels
{
    public class ActivityViewModel
    {
        public long Id { get; set; }
        public long GeneralObjectiveId { get; set; }
        public long SpecificObjectiveId { get; set; }
        public BasicViewModel Department { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string State { get; set; }
        public string Detail { get; set; }
        public string ModificationMotive { get; set; }
        public BasicViewModel CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public BasicViewModel ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}