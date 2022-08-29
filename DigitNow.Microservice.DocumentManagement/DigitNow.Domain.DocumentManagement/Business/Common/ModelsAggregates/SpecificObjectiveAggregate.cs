using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Data.Entities.Objectives;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates
{
    public class SpecificObjectiveAggregate
    {
        public SpecificObjective SpecificObjective { get; set; }
        public List<DocumentFileMappingModel> DocumentFileMappingModels { get; set; }
    }
}