using DigitNow.Domain.DocumentManagement.Business.Common.Filters.Abstractions;
using DigitNow.Domain.DocumentManagement.Data.Filters.Procedures;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Procedures
{
    internal class ProceduresFilterComponentContext: IDataExpressionFilterComponentContext
    { 
        public ProcedureFilter ProcedureFilter { get; set; }
    }
}
