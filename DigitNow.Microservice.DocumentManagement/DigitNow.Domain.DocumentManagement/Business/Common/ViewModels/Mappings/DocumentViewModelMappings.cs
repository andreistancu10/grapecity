using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;
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
                .ForMember(c => c.Recipient, opt => opt.MapFrom<MapDocumentRecipient>())
                .ForMember(c => c.Issuer, opt => opt.MapFrom<MapDocumentIssuer>())
                .ForMember(c => c.Status, opt => opt.MapFrom<MapDocumentStatus>())
                .ForMember(c => c.DocumentType, opt => opt.MapFrom<MapDocumentType>())
                .ForMember(c => c.ResolutionPeriod, opt => opt.MapFrom<MapDocumentResolutionPeriod>())
                .ForMember(c => c.User, opt => opt.MapFrom<MapDocumentUser>())
                .ForMember(c => c.DocumentCategory, opt => opt.MapFrom<MapDocumentCategory>())
                .ForMember(c => c.IsDispatched, opt => opt.MapFrom<MapDocumentIsDispatched>())
                .ForMember(c => c.IdentificationNumber, opt => opt.MapFrom(src => src.VirtualDocument.IdentificationNumber));

            CreateMap<VirtualDocumentAggregate<OutgoingDocument>, DocumentViewModel>()
                .ForMember(c => c.DocumentId, opt => opt.MapFrom(src => src.VirtualDocument.Document.Id))
                .ForMember(c => c.VirtualDocumentId, opt => opt.MapFrom(src => src.VirtualDocument.Id))
                .ForMember(c => c.RegistrationDate, opt => opt.MapFrom(src => src.VirtualDocument.Document.RegistrationDate))
                .ForMember(c => c.RegistrationNumber, opt => opt.MapFrom(src => src.VirtualDocument.Document.RegistrationNumber))
                .ForMember(c => c.Recipient, opt => opt.MapFrom<MapDocumentRecipient>())
                .ForMember(c => c.Issuer, opt => opt.MapFrom<MapDocumentIssuer>())
                .ForMember(c => c.Status, opt => opt.MapFrom<MapDocumentStatus>())
                .ForMember(c => c.DocumentType, opt => opt.MapFrom<MapDocumentType>())
                .ForMember(c => c.User, opt => opt.MapFrom<MapDocumentUser>())
                .ForMember(c => c.ResolutionPeriod, opt => opt.MapFrom<MapDocumentResolutionPeriod>())
                .ForMember(c => c.DocumentCategory, opt => opt.MapFrom<MapDocumentCategory>())
                .ForMember(c => c.IsDispatched, opt => opt.MapFrom<MapDocumentIsDispatched>())
                .ForMember(c => c.IdentificationNumber, opt => opt.MapFrom(src => src.VirtualDocument.IdentificationNumber));

            CreateMap<VirtualDocumentAggregate<InternalDocument>, DocumentViewModel>()
                .ForMember(c => c.DocumentId, opt => opt.MapFrom(src => src.VirtualDocument.Document.Id))
                .ForMember(c => c.VirtualDocumentId, opt => opt.MapFrom(src => src.VirtualDocument.Id))
                .ForMember(c => c.RegistrationDate, opt => opt.MapFrom(src => src.VirtualDocument.Document.RegistrationDate))
                .ForMember(c => c.RegistrationNumber, opt => opt.MapFrom(src => src.VirtualDocument.Document.RegistrationNumber))
                .ForMember(c => c.Recipient, opt => opt.MapFrom<MapDocumentRecipient>())
                .ForMember(c => c.Issuer, opt => opt.MapFrom<MapDocumentIssuer>())
                .ForMember(c => c.Status, opt => opt.MapFrom<MapDocumentStatus>())
                .ForMember(c => c.DocumentType, opt => opt.MapFrom<MapDocumentType>())
                .ForMember(c => c.User, opt => opt.MapFrom<MapDocumentUser>())
                .ForMember(c => c.ResolutionPeriod, opt => opt.MapFrom<MapDocumentResolutionPeriod>())
                .ForMember(c => c.DocumentCategory, opt => opt.MapFrom<MapDocumentCategory>());
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

        private class MapDocumentRecipient :
            IValueResolver<VirtualDocumentAggregate<IncomingDocument>, DocumentViewModel, BasicViewModel>,
            IValueResolver<VirtualDocumentAggregate<InternalDocument>, DocumentViewModel, BasicViewModel>,
            IValueResolver<VirtualDocumentAggregate<OutgoingDocument>, DocumentViewModel, BasicViewModel>
        {
            public BasicViewModel Resolve(VirtualDocumentAggregate<IncomingDocument> source, DocumentViewModel destination, BasicViewModel destMember, ResolutionContext context) =>
                ExtractDepartment(source);

            public BasicViewModel Resolve(VirtualDocumentAggregate<InternalDocument> source, DocumentViewModel destination, BasicViewModel destMember, ResolutionContext context) =>
                ExtractDepartment(source);

            public BasicViewModel Resolve(VirtualDocumentAggregate<OutgoingDocument> source, DocumentViewModel destination, BasicViewModel destMember, ResolutionContext context) =>
                ExtractDepartment(source);

            private static BasicViewModel ExtractDepartment<T>(VirtualDocumentAggregate<T> source)
                where T: VirtualDocument
            {
                var foundDepartment = source.Departments.FirstOrDefault(x => x.Id == source.VirtualDocument.Document.DestinationDepartmentId);
                if (foundDepartment != null)
                {
                    return new BasicViewModel(foundDepartment.Id, foundDepartment.Name);
                }
                return default(BasicViewModel);
            }
        }

        private class MapDocumentUser :
            IValueResolver<VirtualDocumentAggregate<IncomingDocument>, DocumentViewModel, BasicViewModel>,
            IValueResolver<VirtualDocumentAggregate<InternalDocument>, DocumentViewModel, BasicViewModel>,
            IValueResolver<VirtualDocumentAggregate<OutgoingDocument>, DocumentViewModel, BasicViewModel>
        {
            public BasicViewModel Resolve(VirtualDocumentAggregate<IncomingDocument> source, DocumentViewModel destination, BasicViewModel destMember, ResolutionContext context) =>
                ExtractUser(source);

            public BasicViewModel Resolve(VirtualDocumentAggregate<InternalDocument> source, DocumentViewModel destination, BasicViewModel destMember, ResolutionContext context) =>
                ExtractUser(source);

            public BasicViewModel Resolve(VirtualDocumentAggregate<OutgoingDocument> source, DocumentViewModel destination, BasicViewModel destMember, ResolutionContext context) =>
                ExtractUser(source);

            private static BasicViewModel ExtractUser<T>(VirtualDocumentAggregate<T> source)
                where T: VirtualDocument
            {
                var foundUser = source.Users.FirstOrDefault(x => x.Id == source.VirtualDocument.Document.CreatedBy);
                if (foundUser != null)
                {
                    return new BasicViewModel(foundUser.Id, $"{foundUser.FirstName} {foundUser.LastName}");
                }
                return default;
            }
        }

        private class MapDocumentIssuer :
            IValueResolver<VirtualDocumentAggregate<IncomingDocument>, DocumentViewModel, BasicViewModel>,
            IValueResolver<VirtualDocumentAggregate<InternalDocument>, DocumentViewModel, BasicViewModel>,
            IValueResolver<VirtualDocumentAggregate<OutgoingDocument>, DocumentViewModel, BasicViewModel>
        {
            public BasicViewModel Resolve(VirtualDocumentAggregate<IncomingDocument> source, DocumentViewModel destination, BasicViewModel destMember, ResolutionContext context) =>
                ExtractUser(source);

            public BasicViewModel Resolve(VirtualDocumentAggregate<InternalDocument> source, DocumentViewModel destination, BasicViewModel destMember, ResolutionContext context) =>
                ExtractUser(source);

            public BasicViewModel Resolve(VirtualDocumentAggregate<OutgoingDocument> source, DocumentViewModel destination, BasicViewModel destMember, ResolutionContext context) =>
                ExtractUser(source);

            private static BasicViewModel ExtractUser<T>(VirtualDocumentAggregate<T> source)
                where T : VirtualDocument
            {
                var foundUser = source.Users.FirstOrDefault(x => x.Id == source.VirtualDocument.Document.CreatedBy);
                if (foundUser != null)
                {
                    return new BasicViewModel(foundUser.Id, $"{foundUser.FirstName} {foundUser.LastName}");
                }
                return default;
            }
        }

        private class MapDocumentCategory :
            IValueResolver<VirtualDocumentAggregate<IncomingDocument>, DocumentViewModel, BasicViewModel>,
            IValueResolver<VirtualDocumentAggregate<OutgoingDocument>, DocumentViewModel, BasicViewModel>,
            IValueResolver<VirtualDocumentAggregate<InternalDocument>, DocumentViewModel, BasicViewModel>
        {
            public BasicViewModel Resolve(VirtualDocumentAggregate<IncomingDocument> source, DocumentViewModel destination, BasicViewModel destMember, ResolutionContext context)
            {
                var foundCategory = source.Categories.FirstOrDefault(x => x.Id == source.VirtualDocument.DocumentTypeId);
                if (foundCategory != null)
                {
                    return new BasicViewModel(foundCategory.Id, foundCategory.Name);
                }
                return default;
            }

            public BasicViewModel Resolve(VirtualDocumentAggregate<OutgoingDocument> source, DocumentViewModel destination, BasicViewModel destMember, ResolutionContext context)
            {
                var foundCategory = source.Categories.FirstOrDefault(x => x.Id == source.VirtualDocument.DocumentTypeId);
                if (foundCategory != null)
                {
                    return new BasicViewModel(foundCategory.Id, foundCategory.Name);
                }
                return default;
            }

            public BasicViewModel Resolve(VirtualDocumentAggregate<InternalDocument> source, DocumentViewModel destination, BasicViewModel destMember, ResolutionContext context)
            {
                var foundCategory = source.InternalCategories.FirstOrDefault(x => x.Id == source.VirtualDocument.InternalDocumentTypeId);
                if (foundCategory != null)
                {
                    return new BasicViewModel(foundCategory.Id, foundCategory.Name);
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
                return (int)source.VirtualDocument.ResolutionPeriod;
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
                return (int)source.VirtualDocument.DeadlineDaysNumber;
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
                return source.VirtualDocument.Document.Status == DocumentStatus.Finalized || source.VirtualDocument.Document.Status == DocumentStatus.InWorkMayorCountersignature;
            }
        }
    }
}