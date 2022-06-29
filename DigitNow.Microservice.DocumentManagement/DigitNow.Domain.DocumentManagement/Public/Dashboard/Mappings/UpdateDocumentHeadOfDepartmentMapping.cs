using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Dashboard.Commands.Update;
using DigitNow.Domain.DocumentManagement.Public.Dashboard.Models;

namespace DigitNow.Domain.DocumentManagement.Public.Dashboard.Mappings
{
    public class UpdateDocumentHeadOfDepartmentMapping : Profile
    {
        public UpdateDocumentHeadOfDepartmentMapping()
        {
            CreateMap<UpdateDocumentHeadofDepartmentRequest, UpdateDocumentHeadOfDepartmentCommand>();
        }
    }
}
