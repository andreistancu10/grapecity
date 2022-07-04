﻿using System.Linq;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Dashboard.Models;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Dashboard.Queries;

public class GetDocumentsMappings : Profile
{
    public GetDocumentsMappings()
    {
        CreateMap<IncomingDocument, DocumentViewModel>()
            .ForMember(c => c.RegistrationDate, opt => opt.MapFrom(src => src.Document.RegistrationDate))
            .ForMember(c => c.RegistrationNumber, opt => opt.MapFrom(src => src.Document.RegistrationNumber))
            .ForMember(c => c.Recipient, opt => opt.MapFrom(src => src.RecipientId))
            .ForMember(c => c.IssuerName, opt => opt.MapFrom(src => src.CreatedBy))
            .ForMember(c => c.Status, opt => opt.MapFrom<GetDocumentResponseIncomingStatusValueResolver>())
            .ForMember(c => c.DocumentCategory, opt => opt.MapFrom(src => src.DocumentTypeId))
            .ForMember(c => c.DocumentType, opt => opt.MapFrom<GetDocumentResponseIncomingDocumentTypeValueResolver>())
            .ForMember(c => c.DocumentCategory, opt => opt.MapFrom(src => src.DocumentTypeId))
            .ForMember(c => c.ResolutionDuration, opt => opt.MapFrom(src => src.ResolutionPeriod));            

        CreateMap<OutgoingDocument, DocumentViewModel>()
            .ForMember(c => c.RegistrationDate, opt => opt.MapFrom(src => src.Document.RegistrationDate))
            .ForMember(c => c.RegistrationNumber, opt => opt.MapFrom(src => src.Document.RegistrationNumber))
            .ForMember(c => c.Recipient, opt => opt.MapFrom(src => src.RecipientName))
            .ForMember(c => c.IssuerName, opt => opt.MapFrom(src => src.CreatedBy))
            .ForMember(c => c.Status, opt => opt.MapFrom<GetDocumentResponseOutgoingStatusValueResolver>())
            .ForMember(c => c.DocumentCategory, opt => opt.MapFrom(src => src.DocumentTypeId))
            .ForMember(c => c.DocumentType, opt => opt.MapFrom<GetDocumentResponseOutgoingDocumentTypeValueResolver>())
            .ForMember(c => c.DocumentCategory, opt => opt.MapFrom(src => src.DocumentTypeId));

        CreateMap<InternalDocument, DocumentViewModel>()
            .BeforeMap((s, d) => d.DocumentType = (int)DocumentType.Internal);

        CreateMap<DocumentViewModel, GetDocumentResponse>();
    }

    private class GetDocumentResponseOutgoingStatusValueResolver : IValueResolver<OutgoingDocument, DocumentViewModel, int>
    {
        public int Resolve(OutgoingDocument source, DocumentViewModel destination, int destMember, ResolutionContext context)
        {
            var foundWorkflowHistory = source.WorkflowHistory
                    .OrderByDescending(c => c.CreationDate)
                    .FirstOrDefault();

            if (foundWorkflowHistory == null)
                return default(int);

            return (int)foundWorkflowHistory.Status;
        }
    }

    private class GetDocumentResponseOutgoingDocumentTypeValueResolver : IValueResolver<OutgoingDocument, DocumentViewModel, int>
    {
        public int Resolve(OutgoingDocument source, DocumentViewModel destination, int destMember, ResolutionContext context)
        {
            return (int)DocumentType.Outgoing;
        }
    }

    private class GetDocumentResponseIncomingStatusValueResolver : IValueResolver<IncomingDocument, DocumentViewModel, int>
    {
        public int Resolve(IncomingDocument source, DocumentViewModel destination, int destMember, ResolutionContext context)
        {
            var foundWorkflowHistory = source.WorkflowHistory
                    .OrderByDescending(c => c.CreationDate)
                    .FirstOrDefault();

            if (foundWorkflowHistory == null)
                return default(int);

            return (int)foundWorkflowHistory.Status;
        }
    }

    private class GetDocumentResponseIncomingDocumentTypeValueResolver : IValueResolver<IncomingDocument, DocumentViewModel, int>
    {
        public int Resolve(IncomingDocument source, DocumentViewModel destination, int destMember, ResolutionContext context)
        {
            return (int)DocumentType.Incoming;
        }
    }
}