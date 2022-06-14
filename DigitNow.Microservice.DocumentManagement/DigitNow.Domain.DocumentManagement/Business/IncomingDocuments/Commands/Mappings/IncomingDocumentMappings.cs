﻿using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Commands.Create;
using DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Commands.Update;
using DigitNow.Domain.DocumentManagement.Data.ConnectedDocuments;
using DigitNow.Domain.DocumentManagement.Data.ContactDetails;
using DigitNow.Domain.DocumentManagement.Data.IncomingDocuments;

namespace DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Commands.Mappings
{
    public class IncomingDocumentMappings : Profile
    {
        public IncomingDocumentMappings()
        {
            CreateMap<CreateIncomingDocumentCommand, IncomingDocument>();
            CreateMap<CreateContactDetailCommand, ContactDetail>();
            CreateMap<IncomingDocument, ConnectedDocument>();
            CreateMap<UpdateIncomingDocumentCommand, IncomingDocument>()
                .ForMember(x => x.RegistrationNumber, opt => opt.Ignore())
                .ForMember(x => x.RegistrationDate, opt => opt.Ignore());

            CreateMap<UpdateContactDetailCommand, ContactDetail>();
        }
    }
}
