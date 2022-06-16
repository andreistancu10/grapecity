using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data.IncomingConnectedDocuments;

namespace DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Queries;

public class GetDocsByRegistrationNumberMapping : Profile
{
    public GetDocsByRegistrationNumberMapping()
    {
        CreateMap<IncomingConnectedDocument, GetDocsByRegistrationNumberResponse>();
    }
}