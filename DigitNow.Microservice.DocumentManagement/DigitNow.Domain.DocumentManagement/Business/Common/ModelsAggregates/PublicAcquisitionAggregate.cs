using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates
{
    public class PublicAcquisitionAggregate
    {
        public PublicAcquisitionProject PublicAcquisitionProject { get; set; }
        public IReadOnlyList<CpvCodeModel> CpvCodes { get; set; }
        public IReadOnlyList<EstablishedProcedureModel> EstablishedProcedures { get; set; }
    }
}
