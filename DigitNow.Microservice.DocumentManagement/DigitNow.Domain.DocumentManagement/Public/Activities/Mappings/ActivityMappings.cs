using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Activities.Commands.Create;
using DigitNow.Domain.DocumentManagement.Business.Activities.Commands.Update;
using DigitNow.Domain.DocumentManagement.Public.Activities.Models;

namespace DigitNow.Domain.DocumentManagement.Public.Activities.Mappings
{
    public class ActivityMappings : Profile
    {
        public ActivityMappings()
        {
            CreateMap<CreateActivityRequest, CreateActivityCommand>();
            CreateMap<UpdateActivityRequest, UpdateActivityCommand>();
        }
    }
}
