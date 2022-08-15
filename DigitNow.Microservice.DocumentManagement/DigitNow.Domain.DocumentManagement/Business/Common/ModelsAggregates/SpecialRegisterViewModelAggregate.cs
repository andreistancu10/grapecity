using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates
{
    public class SpecialRegisterViewModelAggregate
    {
        public SpecialRegister SpecialRegister { get; set; }
        internal IReadOnlyList<DocumentCategoryModel> Categories { get; set; }
    }
}