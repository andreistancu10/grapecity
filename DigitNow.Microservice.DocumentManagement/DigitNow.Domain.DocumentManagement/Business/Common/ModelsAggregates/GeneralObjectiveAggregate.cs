using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates
{
    public class GeneralObjectiveAggregate
    {
        public GeneralObjective GeneralObjective { get; set; }
        public IReadOnlyList<StateModel> States { get; set; }
        public IReadOnlyList<UserModel> Users { get; set; }
    }
}