using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Actions.Commands;
using DigitNow.Domain.DocumentManagement.Public.Actions.Models;

namespace DigitNow.Domain.DocumentManagement.Public.Actions.Mappings
{
    public class ActionMapping : Profile
    {
        public ActionMapping()
        {
            CreateMap<CreateActionRequest, CreateActionCommand>();
        }
    }
}
