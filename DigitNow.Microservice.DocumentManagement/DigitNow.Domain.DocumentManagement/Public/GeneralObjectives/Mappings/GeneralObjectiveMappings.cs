using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.GeneralObjectives.Commands.Create;
using DigitNow.Domain.DocumentManagement.Business.GeneralObjectives.Commands.Update;
using DigitNow.Domain.DocumentManagement.Public.GeneralObjectives.Models;

namespace DigitNow.Domain.DocumentManagement.Public.GeneralObjectives.Mappings
{
    public class GeneralObjectiveMappings: Profile
    {
        public GeneralObjectiveMappings()
        {
           CreateMap<CreateGeneralObjectiveRequest, CreateGeneralObjectiveCommand>();
           CreateMap<UpdateGeneralObjectiveRequest, UpdateGeneralObjectiveCommand>();
        }
    }
}
