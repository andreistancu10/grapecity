using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Export
{
    internal class ExportMappings : Profile
    {
        public ExportMappings()
        {
            CreateMap<DocumentViewModel, ExportDocumentViewModel>()
                .ForMember(dest => dest.Issuer, opt => opt.MapFrom(src => src.Issuer.Name))
                .ForMember(dest => dest.Recipient, opt => opt.MapFrom(src => src.Recipient.Name))
                .ForMember(dest => dest.DocumentCategory, opt => opt.MapFrom(src => src.DocumentCategory.Name))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User.Name));
        }
    }
}
