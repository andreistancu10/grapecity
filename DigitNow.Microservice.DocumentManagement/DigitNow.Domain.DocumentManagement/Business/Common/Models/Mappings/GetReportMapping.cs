using System;
using System.Linq;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Reports.Queries;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Models.Mappings;

public class GetReportMapping : Profile
{
    public GetReportMapping()
    {
        CreateMap<VirtualDocumentAggregate<IncomingDocument>, GetReportResponse>()
            .ForMember(c => c.Id, opt => opt.MapFrom(src => src.VirtualDocument.Document.Id))
            .ForMember(c => c.RegistrationDate, opt => opt.MapFrom(src => src.VirtualDocument.Document.RegistrationDate))
            .ForMember(c => c.RegistrationNumber, opt => opt.MapFrom(src => src.VirtualDocument.Document.RegistrationNumber))
            .ForMember(c => c.Recipient, opt => opt.MapFrom(src => src.VirtualDocument.RecipientId))
            .ForMember(c => c.IssuerName, opt => opt.MapFrom(src => src.VirtualDocument.CreatedBy))
            .ForMember(c => c.CurrentStatus, opt => opt.MapFrom<MapDocumentCurrentStatus>())
            .ForMember(c => c.DocumentType, opt => opt.MapFrom<MapDocumentType>())
            .ForMember(c => c.DocumentCategoryId, opt => opt.MapFrom(src => src.VirtualDocument.DocumentTypeId)) //TODO: which one?
            .ForMember(c => c.DocumentCategory, opt => opt.MapFrom<MapDocumentCategory>()) //TODO: which one?
                                                                                           //.ForMember(c => c.User, opt => opt.MapFrom<MapUserFromAggregate>())
            .ForMember(c => c.Functionary, opt => opt.MapFrom<MapFunctionary>())
            .ForMember(c => c.AllocationDate, opt => opt.MapFrom<MapAllocationDate>())
            .ForMember(c => c.ResolutionDate, opt => opt.MapFrom<MapResolutionDate>())
            .ForMember(c => c.Expires, opt => opt.MapFrom<MapExpires>())
            .ForMember(c => c.SpecialRegisterId, opt => opt.MapFrom<MapSpecialRegisterId>());

        //CreateMap<VirtualDocumentAggregate<InternalDocument>, GetReportResponse>()
        //    .ForMember(c => c.Id, opt => opt.MapFrom(src => src.VirtualDocument.Document.Id))
        //    .ForMember(c => c.RegistrationDate, opt => opt.MapFrom(src => src.VirtualDocument.Document.RegistrationDate))
        //    .ForMember(c => c.RegistrationNumber, opt => opt.MapFrom(src => src.VirtualDocument.Document.RegistrationNumber))
        //    .ForMember(c => c.Recipient, opt => opt.MapFrom(src => src.VirtualDocument.RecipientName))
        //    .ForMember(c => c.IssuerName, opt => opt.MapFrom(src => src.VirtualDocument.CreatedBy))
        //    .ForMember(c => c.Status, opt => opt.MapFrom<MapDocumentCurrentStatus>())
        //    .ForMember(c => c.DocumentType, opt => opt.MapFrom<MapDocumentType>())
        //    .ForMember(c => c.User, opt => opt.MapFrom<MapUserFromAggregate>())
        //    .ForMember(c => c.DocumentCategoryId, opt => opt.MapFrom<MapDocumentCategory>());

        //CreateMap<VirtualDocumentAggregate<OutgoingDocument>, GetReportResponse>()
        //    .ForMember(c => c.Id, opt => opt.MapFrom(src => src.VirtualDocument.Document.Id))
        //    .ForMember(c => c.DocumentType, opt => opt.MapFrom<MapDocumentType>())
        //    .ForMember(c => c.User, opt => opt.MapFrom<MapUserFromAggregate>())
        //    .ForMember(c => c.DocumentCategoryId, opt => opt.MapFrom<MapDocumentCategory>());

    }

    private class MapFunctionary : IValueResolver<VirtualDocumentAggregate<IncomingDocument>, GetReportResponse, string>
    {
        public string Resolve(VirtualDocumentAggregate<IncomingDocument> source, GetReportResponse destination, string destMember, ResolutionContext context)
        {
            return source.VirtualDocument.WorkflowHistory.Last(c=>c.Status == DocumentStatus.InWorkAllocated).RecipientName;
        }
    }

    private class MapAllocationDate : IValueResolver<VirtualDocumentAggregate<IncomingDocument>, GetReportResponse, DateTime>
    {
        public DateTime Resolve(VirtualDocumentAggregate<IncomingDocument> source, GetReportResponse destination, DateTime destMember, ResolutionContext context)
        {
            return source.VirtualDocument.WorkflowHistory.Last(c=>c.Status == DocumentStatus.InWorkAllocated).CreationDate;
        }
    }

    private class MapResolutionDate : IValueResolver<VirtualDocumentAggregate<IncomingDocument>, GetReportResponse, DateTime>
    {
        public DateTime Resolve(VirtualDocumentAggregate<IncomingDocument> source, GetReportResponse destination, DateTime destMember, ResolutionContext context)
        {
            throw new System.NotImplementedException();
        }
    }

    private class MapExpires : IValueResolver<VirtualDocumentAggregate<IncomingDocument>, GetReportResponse, int>
    {
        public int Resolve(VirtualDocumentAggregate<IncomingDocument> source, GetReportResponse destination, int destMember, ResolutionContext context)
        {
            //TODO
            throw new System.NotImplementedException();
        }
    }

    private class MapSpecialRegisterId : IValueResolver<VirtualDocumentAggregate<IncomingDocument>, GetReportResponse, int>
    {
        public int Resolve(VirtualDocumentAggregate<IncomingDocument> source, GetReportResponse destination, int destMember, ResolutionContext context)
        {
            //TODO
            throw new System.NotImplementedException();
        }
    }





    private class MapDocumentType :
        IValueResolver<VirtualDocumentAggregate<IncomingDocument>, GetReportResponse, int>,
        IValueResolver<VirtualDocumentAggregate<OutgoingDocument>, GetReportResponse, int>,
        IValueResolver<VirtualDocumentAggregate<InternalDocument>, GetReportResponse, int>
    {
        public int Resolve(VirtualDocumentAggregate<IncomingDocument> source, GetReportResponse destination, int destMember, ResolutionContext context) =>
            (int)DocumentType.Incoming;

        public int Resolve(VirtualDocumentAggregate<OutgoingDocument> source, GetReportResponse destination, int destMember, ResolutionContext context) =>
            (int)DocumentType.Outgoing;

        public int Resolve(VirtualDocumentAggregate<InternalDocument> source, GetReportResponse destination, int destMember, ResolutionContext context) =>
            (int)DocumentType.Internal;
    }

    private class MapDocumentCurrentStatus :
        IValueResolver<VirtualDocumentAggregate<IncomingDocument>, GetReportResponse, int>,
        IValueResolver<VirtualDocumentAggregate<OutgoingDocument>, GetReportResponse, int>
    {
        public int Resolve(VirtualDocumentAggregate<IncomingDocument> source, GetReportResponse destination, int destMember, ResolutionContext context) =>
            (int)source.VirtualDocument.Document.Status;

        public int Resolve(VirtualDocumentAggregate<OutgoingDocument> source, GetReportResponse destination, int destMember, ResolutionContext context) =>
            (int)source.VirtualDocument.Document.Status;
    }

    private class MapUserFromAggregate :
        IValueResolver<VirtualDocumentAggregate<IncomingDocument>, GetReportResponse, string>,
        IValueResolver<VirtualDocumentAggregate<InternalDocument>, GetReportResponse, string>,
        IValueResolver<VirtualDocumentAggregate<OutgoingDocument>, GetReportResponse, string>
    {
        public string Resolve(VirtualDocumentAggregate<IncomingDocument> source, GetReportResponse destination, string destMember, ResolutionContext context)
        {
            var foundUser = source.Users.FirstOrDefault(x => x.Id == source.VirtualDocument.Document.CreatedBy);
            if (foundUser != null)
            {
                return $"{foundUser.FirstName} {foundUser.LastName}";
            }
            return default(string);
        }

        public string Resolve(VirtualDocumentAggregate<InternalDocument> source, GetReportResponse destination, string destMember, ResolutionContext context)
        {
            var foundUser = source.Users.FirstOrDefault(x => x.Id == source.VirtualDocument.Document.CreatedBy);
            if (foundUser != null)
            {
                return $"{foundUser.FirstName} {foundUser.LastName}";
            }
            return default(string);
        }

        public string Resolve(VirtualDocumentAggregate<OutgoingDocument> source, GetReportResponse destination, string destMember, ResolutionContext context)
        {
            var foundUser = source.Users.FirstOrDefault(x => x.Id == source.VirtualDocument.Document.CreatedBy);
            if (foundUser != null)
            {
                return $"{foundUser.FirstName} {foundUser.LastName}";
            }
            return default(string);
        }
    }

    private class MapDocumentCategory :
        IValueResolver<VirtualDocumentAggregate<IncomingDocument>, GetReportResponse, string>,
        IValueResolver<VirtualDocumentAggregate<OutgoingDocument>, GetReportResponse, string>,
        IValueResolver<VirtualDocumentAggregate<InternalDocument>, GetReportResponse, string>
    {
        public string Resolve(VirtualDocumentAggregate<IncomingDocument> source, GetReportResponse destination, string destMember, ResolutionContext context)
        {
            var foundCategory = source.Categories.FirstOrDefault(x => x.Id == source.VirtualDocument.DocumentTypeId);
            if (foundCategory != null)
            {
                return foundCategory.Name;
            }
            return default(string);
        }

        public string Resolve(VirtualDocumentAggregate<OutgoingDocument> source, GetReportResponse destination, string destMember, ResolutionContext context)
        {
            var foundCategory = source.Categories.FirstOrDefault(x => x.Id == source.VirtualDocument.DocumentTypeId);
            if (foundCategory != null)
            {
                return foundCategory.Name;
            }
            return default(string);
        }

        public string Resolve(VirtualDocumentAggregate<InternalDocument> source, GetReportResponse destination, string destMember, ResolutionContext context)
        {
            var foundCategory = source.InternalCategories.FirstOrDefault(x => x.Id == source.VirtualDocument.InternalDocumentTypeId);
            if (foundCategory != null)
            {
                return foundCategory.Name;
            }
            return default(string);
        }
    }
}