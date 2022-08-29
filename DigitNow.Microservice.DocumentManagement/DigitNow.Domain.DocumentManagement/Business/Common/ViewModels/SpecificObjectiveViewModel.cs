﻿using DigitNow.Domain.DocumentManagement.Contracts.Objectives;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels
{
    public class SpecificObjectiveViewModel
    {
        public string Code { get; set; }
        public BasicViewModel Department { get; set; }
        public BasicViewModel Functionary { get; set; }
        public string Title { get; set; }
        public BasicViewModel User { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public ObjectiveState State { get; set; }
    }
}