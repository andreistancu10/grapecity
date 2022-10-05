﻿using DigitNow.Domain.DocumentManagement.Contracts.Objectives;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels
{
    public class ActionViewModel
    {
        public long Id { get; set; }
        public BasicViewModel SpecificObjective { get; set; }
        public BasicViewModel GeneralObjective { get; set; }
        public BasicViewModel Activity { get; set; }
        public BasicViewModel Department { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public long StateId{ get; set; }
        public string Risk { get; set; }
        public string ModificationMotive { get; set; }
        public BasicViewModel CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public BasicViewModel ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}