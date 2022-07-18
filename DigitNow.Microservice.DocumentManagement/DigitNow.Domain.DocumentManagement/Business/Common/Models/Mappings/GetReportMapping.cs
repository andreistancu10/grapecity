using System;
using System.Linq;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Models.Mappings;

public class GetReportMapping : Profile
{
    public GetReportMapping()
    {
        CreateMap<VirtualDocumentAggregate<IncomingDocument>, ReportViewModel>()
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

        CreateMap<VirtualDocumentAggregate<InternalDocument>, ReportViewModel>()
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
        IValueResolver<VirtualDocumentAggregate<IncomingDocument>, ReportViewModel, BasicViewModel>,
        IValueResolver<VirtualDocumentAggregate<InternalDocument>, ReportViewModel, BasicViewModel>
    {
        public BasicViewModel Resolve(VirtualDocumentAggregate<IncomingDocument> source, ReportViewModel destination, BasicViewModel destMember, ResolutionContext context)
        {
            var lastWorkflowHistory = source.VirtualDocument.WorkflowHistory.LastOrDefault(c => c.Status == DocumentStatus.InWorkAllocated);

            return lastWorkflowHistory != null ?
             new BasicViewModel(lastWorkflowHistory.RecipientId, lastWorkflowHistory.RecipientName) :
             null;
        }

        public BasicViewModel Resolve(VirtualDocumentAggregate<InternalDocument> source, ReportViewModel destination, BasicViewModel destMember, ResolutionContext context)
        {
            //TODO: should Internal doc have WorkflowHistory?
            var foundUser = source.Users.FirstOrDefault(x => x.Id == source.VirtualDocument.Document.CreatedBy);

            return foundUser != null ?
                new BasicViewModel(foundUser.Id, $"{foundUser.FirstName} {foundUser.LastName}") :
                null;
        }
    }

    private class MapAllocationDate :
        IValueResolver<VirtualDocumentAggregate<IncomingDocument>, ReportViewModel, DateTime?>,
        IValueResolver<VirtualDocumentAggregate<InternalDocument>, ReportViewModel, DateTime?>
    {
        public DateTime? Resolve(VirtualDocumentAggregate<IncomingDocument> source, ReportViewModel destination, DateTime? destMember, ResolutionContext context)
        {
            return source.VirtualDocument.Document.RegistrationDate;
        }

        public DateTime? Resolve(VirtualDocumentAggregate<InternalDocument> source, ReportViewModel destination, DateTime? destMember, ResolutionContext context)
        {
            return source.VirtualDocument.Document.RegistrationDate;
        }
    }

    private class MapResolutionDate :
        IValueResolver<VirtualDocumentAggregate<IncomingDocument>, ReportViewModel, DateTime?>,
        IValueResolver<VirtualDocumentAggregate<InternalDocument>, ReportViewModel, DateTime?>
    {
        public DateTime? Resolve(VirtualDocumentAggregate<IncomingDocument> source, ReportViewModel destination, DateTime? destMember, ResolutionContext context)
        {
            var allocationDate = source.VirtualDocument.Document.RegistrationDate;
            var resolutionDate = allocationDate.AddDays(source.VirtualDocument.ResolutionPeriod);

            return resolutionDate;
        }

        public DateTime? Resolve(VirtualDocumentAggregate<InternalDocument> source, ReportViewModel destination, DateTime? destMember, ResolutionContext context)
        {
            var allocationDate = source.VirtualDocument.Document.RegistrationDate;
            var resolutionPeriod = source.InternalCategories.First(c => c.Id == source.VirtualDocument.InternalDocumentTypeId).ResolutionPeriod;
            var resolutionDate = allocationDate.AddDays(resolutionPeriod);

            return resolutionDate;
        }
    }

    private class MapExpired :
        IValueResolver<VirtualDocumentAggregate<IncomingDocument>, ReportViewModel, int>,
        IValueResolver<VirtualDocumentAggregate<InternalDocument>, ReportViewModel, int>
    {
        public int Resolve(VirtualDocumentAggregate<IncomingDocument> source, ReportViewModel destination, int destMember, ResolutionContext context)
        {
            var allocationDate = source.VirtualDocument.Document.RegistrationDate;
            var resolutionDate = allocationDate.AddDays(source.VirtualDocument.ResolutionPeriod);
            var expired = DateTime.Now - resolutionDate;

            return Math.Abs(expired.Days);
        }

        public int Resolve(VirtualDocumentAggregate<InternalDocument> source, ReportViewModel destination, int destMember, ResolutionContext context)
        {
            var resolutionPeriod = source.InternalCategories.First(c => c.Id == source.VirtualDocument.InternalDocumentTypeId).ResolutionPeriod;
            var allocationDate = source.VirtualDocument.Document.RegistrationDate;
            var resolutionDate = allocationDate.AddDays(resolutionPeriod);
            var expired = DateTime.UtcNow - resolutionDate;

            return expired.Days;
        }
    }

    private class MapSpecialRegister :
        IValueResolver<VirtualDocumentAggregate<IncomingDocument>, ReportViewModel, BasicViewModel>,
        IValueResolver<VirtualDocumentAggregate<InternalDocument>, ReportViewModel, BasicViewModel>
    {
        public BasicViewModel Resolve(VirtualDocumentAggregate<IncomingDocument> source, ReportViewModel destination, BasicViewModel destMember, ResolutionContext context)
        {
            return source.SpecialRegisterMapping != null ?
                new BasicViewModel(source.SpecialRegisterMapping.SpecialRegisterId, source.SpecialRegisterMapping?.SpecialRegister.Name) :
                null;
        }

        public BasicViewModel Resolve(VirtualDocumentAggregate<InternalDocument> source, ReportViewModel destination, BasicViewModel destMember, ResolutionContext context)
        {
            return source.SpecialRegisterMapping != null ?
                new BasicViewModel(source.SpecialRegisterMapping.SpecialRegisterId, source.SpecialRegisterMapping?.SpecialRegister.Name) :
                null;
        }
    }

    private class MapDocumentType :
        IValueResolver<VirtualDocumentAggregate<IncomingDocument>, ReportViewModel, int>,
        IValueResolver<VirtualDocumentAggregate<InternalDocument>, ReportViewModel, int>
    {
        public int Resolve(VirtualDocumentAggregate<IncomingDocument> source, ReportViewModel destination, int destMember, ResolutionContext context) =>
            (int)DocumentType.Incoming;

        public int Resolve(VirtualDocumentAggregate<InternalDocument> source, ReportViewModel destination, int destMember, ResolutionContext context) =>
            (int)DocumentType.Internal;
    }

    private class MapDocumentCurrentStatus :
        IValueResolver<VirtualDocumentAggregate<IncomingDocument>, ReportViewModel, int?>,
        IValueResolver<VirtualDocumentAggregate<InternalDocument>, ReportViewModel, int?>
    {
        public int? Resolve(VirtualDocumentAggregate<IncomingDocument> source, ReportViewModel destination, int? destMember, ResolutionContext context) =>
            (int)source.VirtualDocument.Document.Status;

        public int? Resolve(VirtualDocumentAggregate<InternalDocument> source, ReportViewModel destination, int? destMember, ResolutionContext context) =>
            null;
    }

    private class MapUserFromAggregate :
        IValueResolver<VirtualDocumentAggregate<IncomingDocument>, ReportViewModel, BasicViewModel>,
        IValueResolver<VirtualDocumentAggregate<InternalDocument>, ReportViewModel, BasicViewModel>
    {
        public BasicViewModel Resolve(VirtualDocumentAggregate<IncomingDocument> source, ReportViewModel destination, BasicViewModel destMember, ResolutionContext context)
        {
            var foundUser = source.Users.FirstOrDefault(x => x.Id == source.VirtualDocument.Document.CreatedBy);

            return foundUser != null ?
                new BasicViewModel(foundUser.Id, $"{foundUser.FirstName} {foundUser.LastName}") :
                null;
        }

        public BasicViewModel Resolve(VirtualDocumentAggregate<InternalDocument> source, ReportViewModel destination, BasicViewModel destMember, ResolutionContext context)
        {
            var foundUser = source.Users.FirstOrDefault(x => x.Id == source.VirtualDocument.Document.CreatedBy);

            return foundUser != null ?
                new BasicViewModel(foundUser.Id, $"{foundUser.FirstName} {foundUser.LastName}") :
                null;
        }
    }

    private class MapDocumentCategory :
        IValueResolver<VirtualDocumentAggregate<IncomingDocument>, ReportViewModel, BasicViewModel>,
        IValueResolver<VirtualDocumentAggregate<InternalDocument>, ReportViewModel, BasicViewModel>
    {
        public BasicViewModel Resolve(VirtualDocumentAggregate<IncomingDocument> source, ReportViewModel destination, BasicViewModel destMember, ResolutionContext context)
        {
            var foundCategory = source.Categories.FirstOrDefault(x => x.Id == source.VirtualDocument.DocumentTypeId);
            return new BasicViewModel(foundCategory.Id, foundCategory.Name);
        }

        public BasicViewModel Resolve(VirtualDocumentAggregate<InternalDocument> source, ReportViewModel destination, BasicViewModel destMember, ResolutionContext context)
        {
            var foundCategory = source.InternalCategories.FirstOrDefault(x => x.Id == source.VirtualDocument.InternalDocumentTypeId);
            return new BasicViewModel(foundCategory.Id, foundCategory.Name);
        }
    }

    private class MapRecipient :
        IValueResolver<VirtualDocumentAggregate<IncomingDocument>, ReportViewModel, BasicViewModel>,
        IValueResolver<VirtualDocumentAggregate<InternalDocument>, ReportViewModel, BasicViewModel>
    {
        public BasicViewModel Resolve(VirtualDocumentAggregate<IncomingDocument> source, ReportViewModel destination, BasicViewModel destMember,
            ResolutionContext context)
        {
            var foundUser = source.Users.FirstOrDefault(x => x.Id == source.VirtualDocument.RecipientId);

            return foundUser != null ?
                new BasicViewModel(foundUser.Id, $"{foundUser.FirstName} {foundUser.LastName}") :
                null;
        }

        public BasicViewModel Resolve(VirtualDocumentAggregate<InternalDocument> source, ReportViewModel destination, BasicViewModel destMember,
            ResolutionContext context)
        {
            var foundDepartment = source.Departments.FirstOrDefault(x => x.Id == source.VirtualDocument.ReceiverDepartmentId);

            return foundDepartment != null ?
                new BasicViewModel(foundDepartment.Id, foundDepartment.Name) :
                null;
        }
    }
}