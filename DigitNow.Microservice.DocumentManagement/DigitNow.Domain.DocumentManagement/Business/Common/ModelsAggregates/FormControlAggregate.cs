using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates
{
    public class FormControlAggregate
    {
        public List<FormField> FormFields { get; set; }
        public FormFieldMapping FormFieldMapping { get; set; }
    }
}