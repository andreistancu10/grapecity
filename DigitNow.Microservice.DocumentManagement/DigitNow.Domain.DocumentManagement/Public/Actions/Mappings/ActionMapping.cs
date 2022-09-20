using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Actions.Commands;
using DigitNow.Domain.DocumentManagement.Business.Actions.Commands.Create;
using DigitNow.Domain.DocumentManagement.Business.Actions.Commands.Update;
using DigitNow.Domain.DocumentManagement.Business.Actions.Queries.FilterActions;
using DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Actions;
using DigitNow.Domain.DocumentManagement.Data.Filters.Actions;
using DigitNow.Domain.DocumentManagement.Data.Filters.Activities;
using DigitNow.Domain.DocumentManagement.Public.Actions.Models;

namespace DigitNow.Domain.DocumentManagement.Public.Actions.Mappings
{
    public class ActionMapping : Profile
    {
        public ActionMapping()
        {
            CreateMap<CreateActionRequest, CreateActionCommand>();
            CreateMap<UpdateActionRequest, UpdateActionCommand>();
            CreateMap<FilterActionsRequest, FilterActionsQuery>();
            CreateMap<ActionFilterDto, ActionFilter>();
        }
    }
}
