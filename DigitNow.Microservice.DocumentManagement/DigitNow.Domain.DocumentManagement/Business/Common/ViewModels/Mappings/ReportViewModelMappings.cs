using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Entities.Documents;

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
        }

        private class MapFunctionary :
            IValueResolver<VirtualReportAggregate<IncomingDocument>, ReportViewModel, string>,
            IValueResolver<VirtualReportAggregate<InternalDocument>, ReportViewModel, string>
        {
            public string Resolve(VirtualReportAggregate<IncomingDocument> source, ReportViewModel destination, string destMember, ResolutionContext context)
            {
                var lastWorkflowHistory = source.VirtualDocument.Document.WorkflowHistories.LastOrDefault(c => c.DocumentStatus == DocumentStatus.InWorkAllocated);
                return lastWorkflowHistory?.RecipientName;
            }

            public string Resolve(VirtualReportAggregate<InternalDocument> source, ReportViewModel destination, string destMember, ResolutionContext context)
            {
                var lastWorkflowHistory = source.VirtualDocument.Document.WorkflowHistories.LastOrDefault(c => c.DocumentStatus == DocumentStatus.InWorkAllocated);

                return lastWorkflowHistory?.RecipientName;
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
            IValueResolver<VirtualReportAggregate<IncomingDocument>, ReportViewModel, string>,
            IValueResolver<VirtualReportAggregate<InternalDocument>, ReportViewModel, string>
        {
            public string Resolve(VirtualReportAggregate<IncomingDocument> source, ReportViewModel destination, string destMember, ResolutionContext context) =>
                GetSpecialRegister(source);

            public string Resolve(VirtualReportAggregate<InternalDocument> source, ReportViewModel destination, string destMember, ResolutionContext context) =>
                GetSpecialRegister(source);

            private static string GetSpecialRegister<T>(VirtualReportAggregate<T> reportAggregate)
                where T : VirtualDocument
            {
                return reportAggregate.SpecialRegisterMapping?.SpecialRegister.Name;
            }
        }

        private class MapDocumentType :
            IValueResolver<VirtualReportAggregate<IncomingDocument>, ReportViewModel, string>,
            IValueResolver<VirtualReportAggregate<InternalDocument>, ReportViewModel, string>
        {
            public string Resolve(VirtualReportAggregate<IncomingDocument> source, ReportViewModel destination,
                string destMember, ResolutionContext context) =>
                DocumentTypeRo.Intrare.ToString();

            public string Resolve(VirtualReportAggregate<InternalDocument> source, ReportViewModel destination, string destMember, ResolutionContext context) =>
                DocumentTypeRo.Intern.ToString();
        }

        private class MapDocumentCurrentStatus :
            IValueResolver<VirtualReportAggregate<IncomingDocument>, ReportViewModel, string>,
            IValueResolver<VirtualReportAggregate<InternalDocument>, ReportViewModel, string>
        {
            public string Resolve(VirtualReportAggregate<IncomingDocument> source, ReportViewModel destination,
                string destMember, ResolutionContext context) =>
                GetStatus(source.VirtualDocument.Document);
            public string Resolve(VirtualReportAggregate<InternalDocument> source, ReportViewModel destination, string destMember, ResolutionContext context) =>
                GetStatus(source.VirtualDocument.Document);

            private static string GetStatus(Document document)
            {
                return DocumentStatusMapping.FileStatusLabels
                    .FirstOrDefault(c =>
                        c.Key == document.Status)
                    .Value;
            }
        }

        private class MapUserFromAggregate :
            IValueResolver<VirtualReportAggregate<IncomingDocument>, ReportViewModel, string>,
            IValueResolver<VirtualReportAggregate<InternalDocument>, ReportViewModel, string>
        {
            public string Resolve(VirtualReportAggregate<IncomingDocument> source, ReportViewModel destination, string destMember, ResolutionContext context) =>
                GetUserViewModel(source, source.VirtualDocument);

            public string Resolve(VirtualReportAggregate<InternalDocument> source, ReportViewModel destination, string destMember, ResolutionContext context) =>
                GetUserViewModel(source, source.VirtualDocument);

            private static string GetUserViewModel<T>(VirtualReportAggregate<T> documentAggregate, VirtualDocument virtualDocument)
                where T : VirtualDocument
            {
                var foundUser = documentAggregate.Users.FirstOrDefault(x => x.Id == virtualDocument.Document.CreatedBy);

                return foundUser == null
                    ? null
                    : $"{foundUser.FirstName} {foundUser.LastName}";
            }
        }

        private class MapDocumentCategory :
            IValueResolver<VirtualReportAggregate<IncomingDocument>, ReportViewModel, string>,
            IValueResolver<VirtualReportAggregate<InternalDocument>, ReportViewModel, string>
        {
            public string Resolve(VirtualReportAggregate<IncomingDocument> source, ReportViewModel destination, string destMember, ResolutionContext context)
            {
                var foundCategory = source.Categories.FirstOrDefault(x => x.Id == source.VirtualDocument.DocumentTypeId);

                return foundCategory?.Name;
            }

            public string Resolve(VirtualReportAggregate<InternalDocument> source, ReportViewModel destination, string destMember, ResolutionContext context)
            {
                var foundCategory = source.InternalCategories.FirstOrDefault(x => x.Id == source.VirtualDocument.InternalDocumentTypeId);

                return foundCategory?.Name;
            }
        }

        private class MapRecipient :
            IValueResolver<VirtualReportAggregate<IncomingDocument>, ReportViewModel, string>,
            IValueResolver<VirtualReportAggregate<InternalDocument>, ReportViewModel, string>
        {
            public string Resolve(VirtualReportAggregate<IncomingDocument> source, ReportViewModel destination, string destMember,
                ResolutionContext context)
            {
                return FindRecipient(source.Users, source.VirtualDocument.Document.RecipientId);
            }

            public string Resolve(VirtualReportAggregate<InternalDocument> source, ReportViewModel destination, string destMember,
                ResolutionContext context)
            {
                return FindRecipient(source.Users, source.VirtualDocument.Document.RecipientId);
            }

            private static string FindRecipient(IEnumerable<UserModel> users, long? userId)
            {
                if (userId == null)
                {
                    return null;
                }

                var foundUser = users.FirstOrDefault(x => x.Id == userId);

                return foundUser == null
                    ? null
                    : $"{foundUser.FirstName} {foundUser.LastName}";
            }
        }
    }
}