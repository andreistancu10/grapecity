using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Export;
using System;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Mappings
{
    public class ExcelReportViewModelMappings : Profile
    {
        public ExcelReportViewModelMappings()
        {
            CreateMap<ReportViewModel, ExportReportViewModel>()
               .ForMember(c => c.Recipient, opt => opt.MapFrom(src => src.Recipient.Name))
               .ForMember(c => c.Issuer, opt => opt.MapFrom(src => src.Issuer.Name))
               .ForMember(c => c.CurrentStatus, opt => opt.MapFrom(src => src.CurrentStatus.Label))
               .ForMember(c => c.DocumentType, opt => opt.MapFrom(src => src.DocumentType.Label))
               .ForMember(c => c.DocumentCategory, opt => opt.MapFrom(src => src.DocumentCategory.Name))
               .ForMember(c => c.Functionary, opt => opt.MapFrom<MapFunctionary>())
               .ForMember(c => c.SpecialRegister, opt => opt.MapFrom(src => src.SpecialRegister.Name));
        }

        private class MapFunctionary : IValueResolver<ReportViewModel, ExportReportViewModel, string>
        {
            public string Resolve(ReportViewModel source, ExportReportViewModel destination, string destMember, ResolutionContext context)
            {
                if (source.Functionary != null)
                {
                    return source.Functionary.Name;
                }
                return default;
            }
        }
    }
}
