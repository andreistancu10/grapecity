using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Dashboard.Commands.Update;
using DigitNow.Domain.DocumentManagement.Public.Dashboard.Models;

namespace DigitNow.Domain.DocumentManagement.Public.Dashboard.Mappings
{
    public class UpdateDocDepartmentMapping : Profile
    {
        public UpdateDocDepartmentMapping()
        {
            CreateMap<UpdateDocDepartmentRequest, UpdateDocDepartmentCommand>();
        }
    }
}
