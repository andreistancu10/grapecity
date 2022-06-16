using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data.OutgoingDocuments;

namespace DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Queries;

public class GetDocsByRegistrationNumberMapping : Profile
{
    public GetDocsByRegistrationNumberMapping()
    {
        CreateMap<OutgoingDocument, GetDocsByRegistrationNumberResponse>();
    }
}