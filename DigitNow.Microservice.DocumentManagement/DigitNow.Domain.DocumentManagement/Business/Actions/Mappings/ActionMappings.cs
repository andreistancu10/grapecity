using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Actions.Commands;

namespace DigitNow.Domain.DocumentManagement.Business.Actions.Mappings
{
    public class ActionMappings: Profile
    {
        public ActionMappings()
        {
            CreateMap<CreateActionCommand, Data.Entities.Actions.Action>();
        }
    }
}
