﻿namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels
{
    public class FormControlViewModel
    {
        public long Id { get; set; }
        public long FormId { get; set; }
        public int OrderNumber { get; set; }
        public string Label { get; set; }
        public string Key { get; set; }
        public int FieldType { get; set; }
        public bool Required { get; set; }
        public string InitialValue { get; set; }
    }
}