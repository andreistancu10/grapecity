using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Actions.Commands.Create;
using DigitNow.Domain.DocumentManagement.Business.Actions.Queries.FilterActions;
using DigitNow.Domain.DocumentManagement.Business.Actions.Queries.GetById;
using DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Actions;
using DigitNow.Domain.DocumentManagement.Data.Entities.Actions;
using DigitNow.Domain.DocumentManagement.Data.Filters.Actions;

namespace DigitNow.Domain.DocumentManagement.Business.Actions.Mappings
{
    public class ActionMappings : Profile
    {
        public ActionMappings()
        {
            CreateMap<CreateActionCommand, Data.Entities.Action>();
            CreateMap<Data.Entities.Action, GetActionByIdResponse>()
                .ForPath(dest => dest.Activity.Id, opt => opt.MapFrom(src => src.AssociatedActivity.Id))
                .ForPath(dest => dest.Activity.State, opt => opt.MapFrom(src => src.AssociatedActivity.State))
                .ForPath(dest => dest.Activity.Title, opt => opt.MapFrom(src => src.AssociatedActivity.Title))
                .ForPath(dest => dest.Activity.Code, opt => opt.MapFrom(src => src.AssociatedActivity.Code))
                .ForPath(dest => dest.Activity.Details, opt => opt.MapFrom(src => src.AssociatedActivity.Details))
                .ForPath(dest => dest.Activity.DepartmentId, opt => opt.MapFrom(src => src.AssociatedActivity.DepartmentId))
                .ForPath(dest => dest.Activity.ActivityFunctionaries, opt => opt.MapFrom(src => src.AssociatedActivity.ActivityFunctionaries));


            CreateMap<ActionFunctionary, ActionFunctionaryResponse>();
            CreateMap<FilterActionsQuery, ActionFilter>();
        }
    }
}
