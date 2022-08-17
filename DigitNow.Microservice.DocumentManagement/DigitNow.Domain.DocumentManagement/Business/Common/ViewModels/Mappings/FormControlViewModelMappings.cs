using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Mappings
{
    public class FormControlViewModelMappings:Profile
    {
        public FormControlViewModelMappings()
        {
            CreateMap<FormControlAggregate, FormControlViewModel>();
        }
    }
}