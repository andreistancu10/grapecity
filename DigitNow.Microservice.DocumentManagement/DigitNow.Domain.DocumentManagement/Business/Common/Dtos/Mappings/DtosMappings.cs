using AutoMapper;
using DigitNow.Domain.Authentication.Contracts;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Entities.Actions;
using DigitNow.Domain.DocumentManagement.Data.Entities.Activities;
using DigitNow.Domain.DocumentManagement.Data.Entities.Risks;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Dtos.Mappings
{
    public class DtosMappings : Profile
    {
        public DtosMappings()
        {
            CreateMap<DeliveryDetail, DeliveryDetailDto>().ReverseMap();
            CreateMap<WorkflowHistoryLog, WorkflowHistoryLogDto>().ReverseMap();
            CreateMap<ContactDetail, ContactDetailDto>().ReverseMap();
            CreateMap<RiskControlAction, RiskControlActionDto>().ReverseMap();
            CreateMap<UploadedFileMapping, DocumentUploadedFileDto>().ReverseMap();
            CreateMap<Data.Entities.Action, ActionDto>().ReverseMap();
            CreateMap<Activity, ActivityDto>().ReverseMap();
            CreateMap<RiskTrackingReportDto, RiskTrackingReport>().ReverseMap();
            CreateMap<RiskActionProposalDto, RiskActionProposal>().ReverseMap();

            CreateMap<ConnectedDocument, ConnectedDocumentDto>()
                .ForMember(x => x.RegistrationNumber, opt => opt.MapFrom(src => src.Document.RegistrationNumber))
                .ForMember(x => x.DocumentType, opt => opt.MapFrom(src => src.Document.DocumentType));

            CreateMap<ContactDetailDto, ContactDetailModel>()
                .ForMember(x => x.Name, opt => opt.Ignore())
                .ForMember(x => x.WebSite, opt => opt.Ignore())
                .ForMember(x => x.Header, opt => opt.Ignore())
                .ForMember(x => x.IsLegalEntity, opt => opt.Ignore());

            CreateMap<ActionFunctionary, FunctionaryDto>()
               .ForMember(x => x.EntityId, opt => opt.MapFrom(src => src.ActionId));

            CreateMap<ActivityFunctionary, FunctionaryDto>()
               .ForMember(x => x.EntityId, opt => opt.MapFrom(src => src.ActivityId));

            CreateMap<SpecificObjectiveFunctionary, FunctionaryDto>()
               .ForMember(x => x.EntityId, opt => opt.MapFrom(src => src.SpecificObjectiveId));
        }
    }
}
