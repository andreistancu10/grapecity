using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Data.Entities.Objectives;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates
{
    public class VirtualObjectiveAggregate
    {
        public VirtualObjective VirtualObjective { get; set; }
        public List<DocumentFileMappingModel> DocumentFileMappingModels { get; set; }
    }
}