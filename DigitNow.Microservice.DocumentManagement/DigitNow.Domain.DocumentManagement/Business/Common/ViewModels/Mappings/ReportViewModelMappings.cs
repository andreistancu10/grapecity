using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Export;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Mappings
{
    public class ReportViewModelMappings : Profile
    {
        public ReportViewModelMappings()
        {
            CreateMap<VirtualReportAggregate<IncomingDocument>, ReportViewModel>()
                .ForMember(c => c.Id, opt => opt.MapFrom(src => src.VirtualDocument.Document.Id))
                .ForMember(c => c.RegistrationDate, opt => opt.MapFrom(src => src.VirtualDocument.Document.RegistrationDate))
                .ForMember(c => c.RegistrationNumber, opt => opt.MapFrom(src => src.VirtualDocument.Document.RegistrationNumber))
                .ForMember(c => c.Recipient, opt => opt.MapFrom<MapRecipient>())
                .ForMember(c => c.Issuer, opt => opt.MapFrom<MapUserFromAggregate>())
                .ForMember(c => c.CurrentStatus, opt => opt.MapFrom<MapDocumentCurrentStatus>())
                .ForMember(c => c.DocumentType, opt => opt.MapFrom<MapDocumentType>())
                .ForMember(c => c.DocumentCategory, opt => opt.MapFrom<MapDocumentCategory>())
                .ForMember(c => c.Functionary, opt => opt.MapFrom<MapFunctionary>())
                .ForMember(c => c.AllocationDate, opt => opt.MapFrom<MapAllocationDate>())
                .ForMember(c => c.ResolutionDate, opt => opt.MapFrom<MapResolutionDate>())
                .ForMember(c => c.Expired, opt => opt.MapFrom<MapExpired>())
                .ForMember(c => c.SpecialRegister, opt => opt.MapFrom<MapSpecialRegister>());

            CreateMap<VirtualReportAggregate<InternalDocument>, ReportViewModel>()
                .ForMember(c => c.Id, opt => opt.MapFrom(src => src.VirtualDocument.Document.Id))
                .ForMember(c => c.RegistrationDate, opt => opt.MapFrom(src => src.VirtualDocument.Document.RegistrationDate))
                .ForMember(c => c.RegistrationNumber, opt => opt.MapFrom(src => src.VirtualDocument.Document.RegistrationNumber))
                .ForMember(c => c.Recipient, opt => opt.MapFrom<MapRecipient>())
                .ForMember(c => c.Issuer, opt => opt.MapFrom<MapUserFromAggregate>())
                .ForMember(c => c.CurrentStatus, opt => opt.MapFrom<MapDocumentCurrentStatus>())
                .ForMember(c => c.DocumentType, opt => opt.MapFrom<MapDocumentType>())
                .ForMember(c => c.DocumentCategory, opt => opt.MapFrom<MapDocumentCategory>())
                .ForMember(c => c.Functionary, opt => opt.MapFrom<MapFunctionary>())
                .ForMember(c => c.AllocationDate, opt => opt.MapFrom<MapAllocationDate>())
                .ForMember(c => c.ResolutionDate, opt => opt.MapFrom<MapResolutionDate>())
                .ForMember(c => c.Expired, opt => opt.MapFrom<MapExpired>())
                .ForMember(c => c.SpecialRegister, opt => opt.MapFrom<MapSpecialRegister>());
        }

        private class MapFunctionary :
            IValueResolver<VirtualReportAggregate<IncomingDocument>, ReportViewModel, BasicViewModel>,
            IValueResolver<VirtualReportAggregate<InternalDocument>, ReportViewModel, BasicViewModel>            
        {
            public BasicViewModel Resolve(VirtualReportAggregate<IncomingDocument> source, ReportViewModel destination, BasicViewModel destMember, ResolutionContext context)
            {
                var lastWorkflowHistory = source.VirtualDocument.Document.WorkflowHistories.LastOrDefault(c => 
                    c.DocumentStatus == DocumentStatus.InWorkAllocated
                    ||
                    c.DocumentStatus == DocumentStatus.OpinionRequestedAllocated
                    ||
                    c.DocumentStatus == DocumentStatus.InWorkApprovalRequested
                    ||
                    c.DocumentStatus == DocumentStatus.InWorkMayorReview);

                if (lastWorkflowHistory != null)
                {
                    return new BasicViewModel(lastWorkflowHistory.RecipientId, lastWorkflowHistory.RecipientName);
                }
                return default;
            }

            public BasicViewModel Resolve(VirtualReportAggregate<InternalDocument> source, ReportViewModel destination, BasicViewModel destMember, ResolutionContext context)
            {
                var lastWorkflowHistory = source.VirtualDocument.Document.WorkflowHistories.LastOrDefault(c => 
                    c.DocumentStatus == DocumentStatus.InWorkAllocated
                    ||
                    c.DocumentStatus == DocumentStatus.OpinionRequestedAllocated
                    ||
                    c.DocumentStatus == DocumentStatus.InWorkApprovalRequested
                    ||
                    c.DocumentStatus == DocumentStatus.InWorkMayorReview); 

                if (lastWorkflowHistory != null)
                {
                    return new BasicViewModel(lastWorkflowHistory.RecipientId, lastWorkflowHistory.RecipientName); ;
                }
                return default;
            }
        }

        private class MapAllocationDate :
            IValueResolver<VirtualReportAggregate<IncomingDocument>, ReportViewModel, DateTime?>,
            IValueResolver<VirtualReportAggregate<InternalDocument>, ReportViewModel, DateTime?>
        {
            public DateTime? Resolve(VirtualReportAggregate<IncomingDocument> source, ReportViewModel destination, DateTime? destMember, ResolutionContext context)
            {
                return source.VirtualDocument.Document.StatusModifiedAt;
            }

            public DateTime? Resolve(VirtualReportAggregate<InternalDocument> source, ReportViewModel destination, DateTime? destMember, ResolutionContext context)
            {
                return source.VirtualDocument.Document.StatusModifiedAt;
            }
        }

        private class MapResolutionDate :
            IValueResolver<VirtualReportAggregate<IncomingDocument>, ReportViewModel, DateTime?>,
            IValueResolver<VirtualReportAggregate<InternalDocument>, ReportViewModel, DateTime?>
        {
            public DateTime? Resolve(VirtualReportAggregate<IncomingDocument> source, ReportViewModel destination, DateTime? destMember, ResolutionContext context)
            {
                var allocationDate = source.VirtualDocument.Document.RegistrationDate;
                var resolutionDate = allocationDate.AddDays(source.VirtualDocument.ResolutionPeriod);

                return resolutionDate;
            }

            public DateTime? Resolve(VirtualReportAggregate<InternalDocument> source, ReportViewModel destination, DateTime? destMember, ResolutionContext context)
            {
                var allocationDate = source.VirtualDocument.Document.RegistrationDate;
                var resolutionPeriod = source.InternalCategories.First(c => c.Id == source.VirtualDocument.InternalDocumentTypeId).ResolutionPeriod;
                var resolutionDate = allocationDate.AddDays(resolutionPeriod);

                return resolutionDate;
            }
        }

        private class MapExpired :
            IValueResolver<VirtualReportAggregate<IncomingDocument>, ReportViewModel, int>,
            IValueResolver<VirtualReportAggregate<InternalDocument>, ReportViewModel, int>
        {
            public int Resolve(VirtualReportAggregate<IncomingDocument> source, ReportViewModel destination, int destMember, ResolutionContext context)
            {
                var allocationDate = source.VirtualDocument.Document.RegistrationDate;
                if (allocationDate == DateTime.MinValue) return default;

                var resolutionDate = allocationDate.AddDays(source.VirtualDocument.ResolutionPeriod);
                var expired = DateTime.UtcNow - resolutionDate;

                return Math.Abs(expired.Days);
            }

            public int Resolve(VirtualReportAggregate<InternalDocument> source, ReportViewModel destination, int destMember, ResolutionContext context)
            {
                var foundInternalCategory = source.InternalCategories.FirstOrDefault(c => c.Id == source.VirtualDocument.InternalDocumentTypeId);
                if (foundInternalCategory == null) return default;

                var allocationDate = source.VirtualDocument.Document.RegistrationDate;
                var resolutionDate = allocationDate.AddDays(foundInternalCategory.ResolutionPeriod);
                var expired = DateTime.UtcNow - resolutionDate;

                return Math.Abs(expired.Days);
            }
        }

        private class MapSpecialRegister :
            IValueResolver<VirtualReportAggregate<IncomingDocument>, ReportViewModel, BasicViewModel>,
            IValueResolver<VirtualReportAggregate<InternalDocument>, ReportViewModel, BasicViewModel>
        {
            public BasicViewModel Resolve(VirtualReportAggregate<IncomingDocument> source, ReportViewModel destination, BasicViewModel destMember, ResolutionContext context) =>
                GetSpecialRegister(source);

            public BasicViewModel Resolve(VirtualReportAggregate<InternalDocument> source, ReportViewModel destination, BasicViewModel destMember, ResolutionContext context) =>
                GetSpecialRegister(source);

            private static BasicViewModel GetSpecialRegister<T>(VirtualReportAggregate<T> reportAggregate)
                where T : VirtualDocument
            {
                var foundSpecialRegisterMapping = reportAggregate.VirtualDocument.Document.SpecialRegisterMappings.FirstOrDefault();
                if (foundSpecialRegisterMapping == null) return default;

                var foundSpecialRegister = foundSpecialRegisterMapping.SpecialRegister;
                if (foundSpecialRegister != null)
                {
                    return new BasicViewModel(foundSpecialRegister.Id, foundSpecialRegister.Name);
                }
                return null;
            }
        }

        private class MapDocumentType :
            IValueResolver<VirtualReportAggregate<IncomingDocument>, ReportViewModel, DocumentTypeViewModel>,
            IValueResolver<VirtualReportAggregate<InternalDocument>, ReportViewModel, DocumentTypeViewModel>
        {
            public DocumentTypeViewModel Resolve(VirtualReportAggregate<IncomingDocument> source, ReportViewModel destination, DocumentTypeViewModel destMember, ResolutionContext context) =>
                Resolve(source);

            public DocumentTypeViewModel Resolve(VirtualReportAggregate<InternalDocument> source, ReportViewModel destination, DocumentTypeViewModel destMember, ResolutionContext context) =>
                Resolve(source);

            private static DocumentTypeViewModel Resolve<T>(VirtualReportAggregate<T> source) where T : VirtualDocument
            {
                var viewModel = new DocumentTypeViewModel { Id = source.VirtualDocument.Document.DocumentType };

                var foundTranslation = source.DocumentTypeTranslations.FirstOrDefault(x => x.DocumentType == viewModel.Id);
                if (foundTranslation != null)
                {
                    viewModel.Label = foundTranslation.Translation;
                }

                return viewModel;
            }
        }

        private class MapDocumentCurrentStatus :
            IValueResolver<VirtualReportAggregate<IncomingDocument>, ReportViewModel, DocumentStatusViewModel>,
            IValueResolver<VirtualReportAggregate<InternalDocument>, ReportViewModel, DocumentStatusViewModel>            
        {
            public DocumentStatusViewModel Resolve(VirtualReportAggregate<IncomingDocument> source, ReportViewModel destination, DocumentStatusViewModel destMember, ResolutionContext context) =>
                Resolve(source);

            public DocumentStatusViewModel Resolve(VirtualReportAggregate<InternalDocument> source, ReportViewModel destination, DocumentStatusViewModel destMember, ResolutionContext context) =>
                Resolve(source);

            private static DocumentStatusViewModel Resolve<T>(VirtualReportAggregate<T> source)
                where T: VirtualDocument
            {
                var viewModel = new DocumentStatusViewModel { Status = source.VirtualDocument.Document.Status };
                
                var foundTranslation = source.DocumentStatusTranslations.FirstOrDefault(x => x.Status == source.VirtualDocument.Document.Status);
                if (foundTranslation != null)
                {
                    viewModel.Status = foundTranslation.Status;
                }

                return viewModel;
            }
        }

        private class MapUserFromAggregate :
            IValueResolver<VirtualReportAggregate<IncomingDocument>, ReportViewModel, BasicViewModel>,
            IValueResolver<VirtualReportAggregate<InternalDocument>, ReportViewModel, BasicViewModel>,
            IValueResolver<VirtualReportAggregate<OutgoingDocument>, ReportViewModel, BasicViewModel>
        {
            public BasicViewModel Resolve(VirtualReportAggregate<IncomingDocument> source, ReportViewModel destination, BasicViewModel destMember, ResolutionContext context)
            {
                if (source.VirtualDocument is IncomingDocument incomingDocument)
                {
                    return new BasicViewModel(default, incomingDocument.IssuerName);
                }
                return default;
            }

            public BasicViewModel Resolve(VirtualReportAggregate<InternalDocument> source, ReportViewModel destination, BasicViewModel destMember, ResolutionContext context)
            {
                var foundUser = source.Users.FirstOrDefault(x => x.Id == source.VirtualDocument.Document.CreatedBy);
                if (foundUser != null)
                {
                    return new BasicViewModel(foundUser.Id, $"{foundUser.FirstName} {foundUser.LastName}");
                }
                return default;
            }

            public BasicViewModel Resolve(VirtualReportAggregate<OutgoingDocument> source, ReportViewModel destination, BasicViewModel destMember, ResolutionContext context)
            {
                if (source.VirtualDocument is OutgoingDocument outgoingDocument)
                {
                    return new BasicViewModel(default, outgoingDocument.RecipientName);
                }
                return default;
            }
        }

        private class MapDocumentCategory :
            IValueResolver<VirtualReportAggregate<IncomingDocument>, ReportViewModel, BasicViewModel>,
            IValueResolver<VirtualReportAggregate<InternalDocument>, ReportViewModel, BasicViewModel>
        {
            public BasicViewModel Resolve(VirtualReportAggregate<IncomingDocument> source, ReportViewModel destination, BasicViewModel destMember, ResolutionContext context)
            {
                var foundCategory = source.Categories.FirstOrDefault(x => x.Id == source.VirtualDocument.DocumentTypeId);
                if (foundCategory != null)
                {
                    return new BasicViewModel(foundCategory.Id, foundCategory.Name); ;
                }
                return null;
            }

            public BasicViewModel Resolve(VirtualReportAggregate<InternalDocument> source, ReportViewModel destination, BasicViewModel destMember, ResolutionContext context)
            {
                var foundCategory = source.InternalCategories.FirstOrDefault(x => x.Id == source.VirtualDocument.InternalDocumentTypeId);
                if (foundCategory != null)
                {
                    return new BasicViewModel(foundCategory.Id, foundCategory.Name);
                }
                return null;
            }
        }

        private class MapRecipient :
            IValueResolver<VirtualReportAggregate<IncomingDocument>, ReportViewModel, BasicViewModel>,
            IValueResolver<VirtualReportAggregate<InternalDocument>, ReportViewModel, BasicViewModel>
        {
            public BasicViewModel Resolve(VirtualReportAggregate<IncomingDocument> source, ReportViewModel destination, BasicViewModel destMember, ResolutionContext context) 
                => FindRecipient(source);

            public BasicViewModel Resolve(VirtualReportAggregate<InternalDocument> source, ReportViewModel destination, BasicViewModel destMember, ResolutionContext context)
                => FindRecipient(source);

            private static BasicViewModel FindRecipient<T>(VirtualReportAggregate<T> aggregate)
                where T: VirtualDocument
            {
                var departmentId = aggregate.VirtualDocument.Document.DestinationDepartmentId;

                var foundDepartment = aggregate.Departments.FirstOrDefault(x => x.Id == departmentId);
                if (foundDepartment != null)
                {
                    return new BasicViewModel(foundDepartment.Id, foundDepartment.Name);
                }
                return null;
            }
        }
    }
}