﻿using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data.ConnectedDocuments;

namespace DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Queries;

public class GetDocsByRegistrationNumberMapping : Profile
{
    public GetDocsByRegistrationNumberMapping()
    {
        CreateMap<ConnectedDocument, GetDocsByRegistrationNumberResponse>();
    }
}