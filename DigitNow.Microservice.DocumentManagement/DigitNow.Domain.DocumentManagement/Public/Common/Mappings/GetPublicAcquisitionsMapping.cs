using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.PublicAcquisitions.Queries.GetPublicAcquisitions;
using DigitNow.Domain.DocumentManagement.Data.Filters.PublicAcquisitions;
using DigitNow.Domain.DocumentManagement.Public.PublicAcquisitions.Models;

namespace DigitNow.Domain.DocumentManagement.Public.Common.Mappings
{
    public class GetPublicAcquisitionsMapping : Profile
    {
        public GetPublicAcquisitionsMapping()
        {
            CreateMap<GetPublicAcquisitionsRequest, GetPublicAcquisitionsQuery>()
                .ForMember(x => x.Filter, opt => opt.MapFrom(src => src.Filter ?? new PublicAcquisitionFilterDto()));

            CreateMap<PublicAcquisitionFilterDto, PublicAcquisitionFilter>();
            {
                CreateMap<ProjectYearFilterDto, ProjectYearFilter>()
                    .ForMember(m => m.ProjectYear, opt => opt.MapFrom(src => src.ProjectYear));
                CreateMap<TypeFilterDto, TypeFilter>()
                    .ForMember(m => m.Type, opt => opt.MapFrom(src => src.Type));
                CreateMap<CpvCodeFilterDto, CpvCodeFilter>()
                    .ForMember(m => m.CpvCode, opt => opt.MapFrom(src => src.CpvCode));
                CreateMap<FinancingSourceFilterDto, FinancingSourceFilter>()
                    .ForMember(m => m.FinancingSource, opt => opt.MapFrom(src => src.FinancingSource));
                CreateMap<EstablishedProcedureFilterDto, EstablishedProcedureFilter>()
                    .ForMember(m => m.EstablishedProcedure, opt => opt.MapFrom(src => src.EstablishedProcedure));
                CreateMap<EstimatedMonthForInitiatingProcedureFilterDto, EstimatedMonthForInitiatingProcedureFilter>()
                    .ForMember(m => m.EstimatedMonthForInitiatingProcedure, opt => opt.MapFrom(src => src.EstimatedMonthForInitiatingProcedure));
                CreateMap<EstimatedMonthForProcedureAssignmentFilterDto, EstimatedMonthForProcedureAssignmentFilter>()
                    .ForMember(m => m.EstimatedMonthForProcedureAssignment, opt => opt.MapFrom(src => src.EstimatedMonthForProcedureAssignment));
                CreateMap<ProcedureAssignmentMethodFilterDto, ProcedureAssignmentMethodFilter>()
                    .ForMember(m => m.ProcedureAssignmentMethod, opt => opt.MapFrom(src => src.ProcedureAssignmentMethod));
            }
        }
    }
}
