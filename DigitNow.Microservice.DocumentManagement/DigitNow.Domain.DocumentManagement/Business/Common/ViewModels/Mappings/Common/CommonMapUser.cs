using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Mappings.Common
{
    public class CommonMapUser : 
        IValueResolver<ProcedureAggregate, ProcedureViewModel, BasicViewModel>,
        IValueResolver<ProcedureHistoryAggregate, ProcedureHistoryViewModel, BasicViewModel>

    {
        public BasicViewModel Resolve(ProcedureAggregate source, ProcedureViewModel destination, BasicViewModel destMember,
            ResolutionContext context)
        {
            var foundUser = source.Users.FirstOrDefault(c => c.Id == source.Procedure.CreatedBy);

            return foundUser == null
                ? null
                : new BasicViewModel(foundUser.Id, $"{foundUser.FirstName} {foundUser.LastName}");
        }

        public BasicViewModel Resolve(ProcedureHistoryAggregate source, ProcedureHistoryViewModel destination,
            BasicViewModel destMember, ResolutionContext context)
        {
            var foundUser = source.Users.FirstOrDefault(c => c.Id == source.ProcedureHistory.CreatedBy);

            return foundUser == null
                ? null
                : new BasicViewModel(foundUser.Id, $"{foundUser.FirstName} {foundUser.LastName}");
        }
    }
}