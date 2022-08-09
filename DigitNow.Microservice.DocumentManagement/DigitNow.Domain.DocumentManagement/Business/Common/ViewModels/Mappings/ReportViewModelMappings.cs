using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
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
                .ForMember(c => c.Issuer, opt => opt.MapFrom<MapUserFromAggregate>())
                .ForMember(c => c.CurrentStatus, opt => opt.MapFrom<MapDocumentCurrentStatus>())
                .ForMember(c => c.DocumentType, opt => opt.MapFrom<MapDocumentType>())
                .ForMember(c => c.DocumentCategory, opt => opt.MapFrom<MapDocumentCategory>())
                .ForMember(c => c.Functionary, opt => opt.MapFrom<MapFunctionary>())
                .ForMember(c => c.AllocationDate, opt => opt.MapFrom<MapAllocationDate>())
                .ForMember(c => c.ResolutionDate, opt => opt.MapFrom<MapResolutionDate>())
                .ForMember(c => c.Expired, opt => opt.MapFrom<MapExpired>())
                .ForMember(c => c.SpecialRegister, opt => opt.MapFrom<MapSpecialRegister>());

            CreateMap<ReportViewModelAggregate, ExportReportViewModel>()
               .ForMember(c => c.Id, opt => opt.MapFrom(src => src.ReportViewModel.Id))
               .ForMember(c => c.RegistrationDate, opt => opt.MapFrom(src => src.ReportViewModel.RegistrationDate))
               .ForMember(c => c.RegistrationNumber, opt => opt.MapFrom(src => src.ReportViewModel.RegistrationNumber))
               .ForMember(c => c.Recipient, opt => opt.MapFrom(src => src.ReportViewModel.Recipient.Name))
               .ForMember(c => c.Issuer, opt => opt.MapFrom(src => src.ReportViewModel.Issuer.Name))
               .ForMember(c => c.CurrentStatus, opt => opt.MapFrom<MapDocumentCurrentStatus>())
               .ForMember(c => c.DocumentType, opt => opt.MapFrom<MapDocumentType>())
               .ForMember(c => c.DocumentCategory, opt => opt.MapFrom(src => src.ReportViewModel.DocumentCategory.Name))
               .ForMember(c => c.Functionary, opt => opt.MapFrom<MapFunctionary>())
               .ForMember(c => c.AllocationDate, opt => opt.MapFrom(src => src.ReportViewModel.AllocationDate))
               .ForMember(c => c.ResolutionDate, opt => opt.MapFrom(src => src.ReportViewModel.ResolutionDate))
               .ForMember(c => c.Expired, opt => opt.MapFrom(src => src.ReportViewModel.Expired))
               .ForMember(c => c.SpecialRegister, opt => opt.MapFrom(src => src.ReportViewModel.SpecialRegister.Name));
        }

        private class MapFunctionary :
            IValueResolver<VirtualReportAggregate<IncomingDocument>, ReportViewModel, BasicViewModel>,
            IValueResolver<VirtualReportAggregate<InternalDocument>, ReportViewModel, BasicViewModel>,
            IValueResolver<ReportViewModelAggregate, ExportReportViewModel, string>
        {
            public BasicViewModel Resolve(VirtualReportAggregate<IncomingDocument> source, ReportViewModel destination, BasicViewModel destMember, ResolutionContext context)
            {
                var lastWorkflowHistory = source.VirtualDocument.Document.WorkflowHistories.LastOrDefault(c => c.DocumentStatus == DocumentStatus.InWorkAllocated);
                if (lastWorkflowHistory != null)
                {
                    return new BasicViewModel(lastWorkflowHistory.RecipientId, lastWorkflowHistory.RecipientName);
                }
                return null;
            }

            public BasicViewModel Resolve(VirtualReportAggregate<InternalDocument> source, ReportViewModel destination, BasicViewModel destMember, ResolutionContext context)
            {
                var lastWorkflowHistory = source.VirtualDocument.Document.WorkflowHistories.LastOrDefault(c => c.DocumentStatus == DocumentStatus.InWorkAllocated);
                if (lastWorkflowHistory != null)
                {
                    return new BasicViewModel(lastWorkflowHistory.RecipientId, lastWorkflowHistory.RecipientName); ;
                }
                return null;
            }

            public string Resolve(ReportViewModelAggregate source, ExportReportViewModel destination, string destMember, ResolutionContext context)
            {
                var functionary = source.ReportViewModel.Functionary;
                if (functionary != null)
                {
                    return functionary.Name;
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
                return source.VirtualDocument.Document.RegistrationDate;
            }

            public DateTime? Resolve(VirtualReportAggregate<InternalDocument> source, ReportViewModel destination, DateTime? destMember, ResolutionContext context)
            {
                return source.VirtualDocument.Document.RegistrationDate;
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
                var resolutionDate = allocationDate.AddDays(source.VirtualDocument.ResolutionPeriod);
                var expired = DateTime.Now - resolutionDate;

                return Math.Abs(expired.Days);
            }

            public int Resolve(VirtualReportAggregate<InternalDocument> source, ReportViewModel destination, int destMember, ResolutionContext context)
            {
                var resolutionPeriod = source.InternalCategories.First(c => c.Id == source.VirtualDocument.InternalDocumentTypeId).ResolutionPeriod;
                var allocationDate = source.VirtualDocument.Document.RegistrationDate;
                var resolutionDate = allocationDate.AddDays(resolutionPeriod);
                var expired = DateTime.UtcNow - resolutionDate;

                return expired.Days;
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
                if (reportAggregate.SpecialRegisterMapping != null)
                {
                    new BasicViewModel(reportAggregate.SpecialRegisterMapping.SpecialRegisterId, reportAggregate.SpecialRegisterMapping.SpecialRegister.Name);
                }
                return null;
            }
        }

        private class MapDocumentType :
            IValueResolver<VirtualReportAggregate<IncomingDocument>, ReportViewModel, int>,
            IValueResolver<VirtualReportAggregate<InternalDocument>, ReportViewModel, int>,
            IValueResolver<ReportViewModelAggregate, ExportReportViewModel, string>
        {
            public int Resolve(VirtualReportAggregate<IncomingDocument> source, ReportViewModel destination, int destMember, ResolutionContext context) =>
                (int)DocumentType.Incoming;

            public int Resolve(VirtualReportAggregate<InternalDocument> source, ReportViewModel destination, int destMember, ResolutionContext context) =>
                (int)DocumentType.Internal;

            public string Resolve(ReportViewModelAggregate source, ExportReportViewModel destination, string destMember,
                ResolutionContext context)
            {
                if (source.DocumentTypeTranslations.ContainsKey((DocumentType)source.ReportViewModel.DocumentType))
                {
                    return source.DocumentTypeTranslations[(DocumentType)source.ReportViewModel.DocumentType];
                }

                return string.Empty;
            }
        }

        private class MapDocumentCurrentStatus :
            IValueResolver<VirtualReportAggregate<IncomingDocument>, ReportViewModel, int?>,
            IValueResolver<VirtualReportAggregate<InternalDocument>, ReportViewModel, int?>,
            IValueResolver<ReportViewModelAggregate, ExportReportViewModel, string>

        {
            public int? Resolve(VirtualReportAggregate<IncomingDocument> source, ReportViewModel destination, int? destMember, ResolutionContext context) =>
                (int)source.VirtualDocument.Document.Status;

            public int? Resolve(VirtualReportAggregate<InternalDocument> source, ReportViewModel destination, int? destMember, ResolutionContext context) =>
                null;

            public string Resolve(ReportViewModelAggregate source, ExportReportViewModel destination, string destMember,
                ResolutionContext context)
            {
                var currentStatus = source.ReportViewModel.CurrentStatus.GetValueOrDefault();

                if (source.StatusTranslations.ContainsKey((DocumentStatus)currentStatus))
                {
                    return source.StatusTranslations[(DocumentStatus)currentStatus];
                }

                return string.Empty;
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
            public BasicViewModel Resolve(VirtualReportAggregate<IncomingDocument> source, ReportViewModel destination, BasicViewModel destMember,
                ResolutionContext context)
            {
                return FindRecipient(source.Users, source.VirtualDocument.Document.RecipientId);
            }

            public BasicViewModel Resolve(VirtualReportAggregate<InternalDocument> source, ReportViewModel destination, BasicViewModel destMember,
                ResolutionContext context)
            {
                return FindRecipient(source.Users, source.VirtualDocument.Document.RecipientId);
            }

            private static BasicViewModel FindRecipient(IEnumerable<UserModel> users, long? userId)
            {
                if (userId == null)
                {
                    return null;
                }

                var foundUser = users.FirstOrDefault(x => x.Id == userId);
                if (foundUser != null)
                {
                    return new BasicViewModel(foundUser.Id, $"{foundUser.FirstName} {foundUser.LastName}");
                }
                return null;
            }
        }
    }
}