using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Actions.Commands;
using DigitNow.Domain.DocumentManagement.Business.Actions.Queries.GetById;
using DigitNow.Domain.DocumentManagement.Data.Entities.Actions;

namespace DigitNow.Domain.DocumentManagement.Business.Actions.Mappings
{
    public class ActionMappings: Profile
    {
        public ActionMappings()
        {
            CreateMap<CreateActionCommand, Data.Entities.Actions.Action>();
            CreateMap<Data.Entities.Actions.Action, GetActionByIdResponse>();
            CreateMap<ActionFunctionary, ActionFunctionaryResponse>();  
        }
    }
}
