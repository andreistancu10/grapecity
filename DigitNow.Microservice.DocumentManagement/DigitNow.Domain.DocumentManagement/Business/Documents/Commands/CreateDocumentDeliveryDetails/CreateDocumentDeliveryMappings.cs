﻿using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data.Entities.DeliveryDetails;

namespace DigitNow.Domain.DocumentManagement.Business.Documents.Commands.CreateDocumentDeliveryDetails
{
    public class CreateDocumentDeliveryMappings : Profile
    {
        public CreateDocumentDeliveryMappings()
        {
            CreateMap<CreateDocumentDeliveryDetailsCommand, DeliveryDetail>();
        }
    }
}
