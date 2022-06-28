using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels
{
    public class ViewModelsMappings : Profile
    {
        public ViewModelsMappings()
        {
            CreateMap<Document, DashboardDocumentViewModel>();
        }
    }
}
