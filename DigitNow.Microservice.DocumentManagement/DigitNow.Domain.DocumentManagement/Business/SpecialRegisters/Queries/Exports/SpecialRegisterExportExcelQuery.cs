using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Infrastructure.Data.Abstractions;

namespace DigitNow.Domain.DocumentManagement.Business.SpecialRegisters.Queries.Exports
{
    public class SpecialRegisterExportExcelQuery : AbstractFilterModel<SpecialRegister>, IQuery<List<SpecialRegisterExportViewModel>>
    {
        public long? Id { get; set; }
        public string Name { get; set; }
    }
}