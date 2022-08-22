using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.SpecificObjectives.Commands.Create;
using DigitNow.Domain.DocumentManagement.Business.SpecificObjectives.Commands.Update;
using DigitNow.Domain.DocumentManagement.Public.SpecialObjective.Models;
using DigitNow.Domain.DocumentManagement.Public.SpecificObjectives.Models;

namespace DigitNow.Domain.DocumentManagement.Public.SpecialObjective.Mappings
{
    internal class SpecificObjectiveMappings : Profile
    {
        public SpecificObjectiveMappings()
        {
            CreateMap<CreateSpecificObjectiveRequest, CreateSpecificObjectiveCommand>();
            CreateMap<UpdateSpecificObjectiveRequest, UpdateSpecificObjectiveCommand>();
        }
    }
}
