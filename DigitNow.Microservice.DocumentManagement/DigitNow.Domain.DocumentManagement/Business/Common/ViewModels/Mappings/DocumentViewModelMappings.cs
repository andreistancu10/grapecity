using System.Linq;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;
using DigitNow.Domain.DocumentManagement.Business.Dashboard.Queries;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Mappings
{
    public class DocumentViewModelMappings : Profile
    {
        public DocumentViewModelMappings()
        {
            CreateMap<VirtualDocumentAggregate<IncomingDocument>, DocumentViewModel>()
                .ForMember(c => c.DocumentId, opt => opt.MapFrom(src => src.VirtualDocument.Document.Id))
                .ForMember(c => c.VirtualDocumentId, opt => opt.MapFrom(src => src.VirtualDocument.Id))
                .ForMember(c => c.RegistrationDate, opt => opt.MapFrom(src => src.VirtualDocument.Document.RegistrationDate))
                .ForMember(c => c.RegistrationNumber, opt => opt.MapFrom(src => src.VirtualDocument.Document.RegistrationNumber))
                .ForMember(c => c.Recipient, opt => opt.MapFrom(src => src.VirtualDocument.Document.RecipientId))
                .ForMember(c => c.IssuerName, opt => opt.MapFrom(src => src.VirtualDocument.IssuerName))
                .ForMember(c => c.Status, opt => opt.MapFrom<MapDocumentStatus>())
                .ForMember(c => c.DocumentType, opt => opt.MapFrom<MapDocumentType>())
                .ForMember(c => c.ResolutionPeriod, opt => opt.MapFrom<MapDocumentResolutionPeriod>())
                .ForMember(c => c.User, opt => opt.MapFrom<MapUserFromAggregate>())
                .ForMember(c => c.DocumentCategory, opt => opt.MapFrom<MapDocumentCategory>())
                .ForMember(c => c.IsDispatched, opt => opt.MapFrom<MapDocumentIsDispatched>());

            CreateMap<VirtualDocumentAggregate<OutgoingDocument>, DocumentViewModel>()
                .ForMember(c => c.DocumentId, opt => opt.MapFrom(src => src.VirtualDocument.Document.Id))
                .ForMember(c => c.VirtualDocumentId, opt => opt.MapFrom(src => src.VirtualDocument.Id))
                .ForMember(c => c.RegistrationDate, opt => opt.MapFrom(src => src.VirtualDocument.Document.RegistrationDate))
                .ForMember(c => c.RegistrationNumber, opt => opt.MapFrom(src => src.VirtualDocument.Document.RegistrationNumber))
                .ForMember(c => c.Recipient, opt => opt.MapFrom(src => src.VirtualDocument.RecipientName))
                .ForMember(c => c.IssuerName, opt => opt.MapFrom<MapUserFromAggregate>())
                .ForMember(c => c.Status, opt => opt.MapFrom<MapDocumentStatus>())
                .ForMember(c => c.DocumentType, opt => opt.MapFrom<MapDocumentType>())
                .ForMember(c => c.User, opt => opt.MapFrom<MapUserFromAggregate>())
                .ForMember(c => c.ResolutionPeriod, opt => opt.MapFrom<MapDocumentResolutionPeriod>())
                .ForMember(c => c.DocumentCategory, opt => opt.MapFrom<MapDocumentCategory>())
                .ForMember(c => c.IsDispatched, opt => opt.MapFrom<MapDocumentIsDispatched>());

            CreateMap<VirtualDocumentAggregate<InternalDocument>, DocumentViewModel>()
                .ForMember(c => c.DocumentId, opt => opt.MapFrom(src => src.VirtualDocument.Document.Id))
                .ForMember(c => c.VirtualDocumentId, opt => opt.MapFrom(src => src.VirtualDocument.Id))
                .ForMember(c => c.RegistrationDate, opt => opt.MapFrom(src => src.VirtualDocument.Document.RegistrationDate))
                .ForMember(c => c.RegistrationNumber, opt => opt.MapFrom(src => src.VirtualDocument.Document.RegistrationNumber))
                .ForMember(c => c.IssuerName, opt => opt.MapFrom<MapUserFromAggregate>())
                .ForMember(c => c.Status, opt => opt.MapFrom<MapDocumentStatus>())
                .ForMember(c => c.DocumentType, opt => opt.MapFrom<MapDocumentType>())
                .ForMember(c => c.User, opt => opt.MapFrom<MapUserFromAggregate>())
                .ForMember(c => c.ResolutionPeriod, opt => opt.MapFrom<MapDocumentResolutionPeriod>())
                .ForMember(c => c.DocumentCategory, opt => opt.MapFrom<MapDocumentCategory>());

            CreateMap<DocumentViewModel, GetDocumentResponse>();
        }

        private class MapDocumentType :
            IValueResolver<VirtualDocumentAggregate<IncomingDocument>, DocumentViewModel, int>,
            IValueResolver<VirtualDocumentAggregate<OutgoingDocument>, DocumentViewModel, int>,
            IValueResolver<VirtualDocumentAggregate<InternalDocument>, DocumentViewModel, int>
        {
            public int Resolve(VirtualDocumentAggregate<IncomingDocument> source, DocumentViewModel destination, int destMember, ResolutionContext context) =>
                (int)DocumentType.Incoming;

            public int Resolve(VirtualDocumentAggregate<OutgoingDocument> source, DocumentViewModel destination, int destMember, ResolutionContext context) =>
                (int)DocumentType.Outgoing;

            public int Resolve(VirtualDocumentAggregate<InternalDocument> source, DocumentViewModel destination, int destMember, ResolutionContext context) =>
                (int)DocumentType.Internal;
        }

        private class MapDocumentStatus :
            IValueResolver<VirtualDocumentAggregate<IncomingDocument>, DocumentViewModel, int>,
            IValueResolver<VirtualDocumentAggregate<OutgoingDocument>, DocumentViewModel, int>,
            IValueResolver<VirtualDocumentAggregate<InternalDocument>, DocumentViewModel, int>
        {
            public int Resolve(VirtualDocumentAggregate<IncomingDocument> source, DocumentViewModel destination, int destMember, ResolutionContext context) =>
                (int)source.VirtualDocument.Document.Status;

            public int Resolve(VirtualDocumentAggregate<OutgoingDocument> source, DocumentViewModel destination, int destMember, ResolutionContext context) =>
                (int)source.VirtualDocument.Document.Status;

            public int Resolve(VirtualDocumentAggregate<InternalDocument> source, DocumentViewModel destination, int destMember, ResolutionContext context) =>
                (int)source.VirtualDocument.Document.Status;
        }

        private class MapUserFromAggregate :
            IValueResolver<VirtualDocumentAggregate<IncomingDocument>, DocumentViewModel, string>,
            IValueResolver<VirtualDocumentAggregate<InternalDocument>, DocumentViewModel, string>,
            IValueResolver<VirtualDocumentAggregate<OutgoingDocument>, DocumentViewModel, string>
        {
            public string Resolve(VirtualDocumentAggregate<IncomingDocument> source, DocumentViewModel destination, string destMember, ResolutionContext context)
            {
                var foundUser = source.Users.FirstOrDefault(x => x.Id == source.VirtualDocument.Document.CreatedBy);
                if (foundUser != null)
                {
                    return $"{foundUser.FirstName} {foundUser.LastName}";
                }
                return default;
            }

            public string Resolve(VirtualDocumentAggregate<InternalDocument> source, DocumentViewModel destination, string destMember, ResolutionContext context)
            {
                var foundUser = source.Users.FirstOrDefault(x => x.Id == source.VirtualDocument.Document.CreatedBy);
                if (foundUser != null)
                {
                    return $"{foundUser.FirstName} {foundUser.LastName}";
                }
                return default;
            }

            public string Resolve(VirtualDocumentAggregate<OutgoingDocument> source, DocumentViewModel destination, string destMember, ResolutionContext context)
            {
                var foundUser = source.Users.FirstOrDefault(x => x.Id == source.VirtualDocument.Document.CreatedBy);
                if (foundUser != null)
                {
                    return $"{foundUser.FirstName} {foundUser.LastName}";
                }
                return default;
            }
        }

        private class MapDocumentCategory :
            IValueResolver<VirtualDocumentAggregate<IncomingDocument>, DocumentViewModel, string>,
            IValueResolver<VirtualDocumentAggregate<OutgoingDocument>, DocumentViewModel, string>,
            IValueResolver<VirtualDocumentAggregate<InternalDocument>, DocumentViewModel, string>
        {
            public string Resolve(VirtualDocumentAggregate<IncomingDocument> source, DocumentViewModel destination, string destMember, ResolutionContext context)
            {
                var foundCategory = source.Categories.FirstOrDefault(x => x.Id == source.VirtualDocument.DocumentTypeId);
                if (foundCategory != null)
                {
                    return foundCategory.Name;
                }
                return default;
            }

            public string Resolve(VirtualDocumentAggregate<OutgoingDocument> source, DocumentViewModel destination, string destMember, ResolutionContext context)
            {
                var foundCategory = source.Categories.FirstOrDefault(x => x.Id == source.VirtualDocument.DocumentTypeId);
                if (foundCategory != null)
                {
                    return foundCategory.Name;
                }
                return default;
            }

            public string Resolve(VirtualDocumentAggregate<InternalDocument> source, DocumentViewModel destination, string destMember, ResolutionContext context)
            {
                var foundCategory = source.InternalCategories.FirstOrDefault(x => x.Id == source.VirtualDocument.InternalDocumentTypeId);
                if (foundCategory != null)
                {
                    return foundCategory.Name;
                }
                return default;
            }
        }

        private class MapDocumentResolutionPeriod :
            IValueResolver<VirtualDocumentAggregate<IncomingDocument>, DocumentViewModel, int>,
            IValueResolver<VirtualDocumentAggregate<OutgoingDocument>, DocumentViewModel, int>,
            IValueResolver<VirtualDocumentAggregate<InternalDocument>, DocumentViewModel, int>
        {
            public int Resolve(VirtualDocumentAggregate<IncomingDocument> source, DocumentViewModel destination, int destMember, ResolutionContext context)
            {
                var foundCategory = source.Categories.FirstOrDefault(x => x.Id == source.VirtualDocument.DocumentTypeId);
                if (foundCategory != null)
                {
                    return foundCategory.ResolutionPeriod;
                }
                return default;
            }

            public int Resolve(VirtualDocumentAggregate<OutgoingDocument> source, DocumentViewModel destination, int destMember, ResolutionContext context)
            {
                var foundCategory = source.Categories.FirstOrDefault(x => x.Id == source.VirtualDocument.DocumentTypeId);
                if (foundCategory != null)
                {
                    return foundCategory.ResolutionPeriod;
                }
                return default;
            }

            public int Resolve(VirtualDocumentAggregate<InternalDocument> source, DocumentViewModel destination, int destMember, ResolutionContext context)
            {
                var foundCategory = source.InternalCategories.FirstOrDefault(x => x.Id == source.VirtualDocument.InternalDocumentTypeId);
                if (foundCategory != null)
                {
                    return foundCategory.ResolutionPeriod;
                }
                return default;
            }
        }

        private class MapDocumentIsDispatched :
            IValueResolver<VirtualDocumentAggregate<IncomingDocument>, DocumentViewModel, bool>,
            IValueResolver<VirtualDocumentAggregate<OutgoingDocument>, DocumentViewModel, bool>
        {
            public bool Resolve(VirtualDocumentAggregate<IncomingDocument> source, DocumentViewModel destination, bool destMember, ResolutionContext context)
            {
                return source.VirtualDocument.Document.Status == DocumentStatus.Finalized;
            }

            public bool Resolve(VirtualDocumentAggregate<OutgoingDocument> source, DocumentViewModel destination, bool destMember, ResolutionContext context)
            {
                return source.VirtualDocument.Document.Status == DocumentStatus.Finalized || source.VirtualDocument.Document.Status == DocumentStatus.InWorkCountersignature;
            }
        }
    }
}