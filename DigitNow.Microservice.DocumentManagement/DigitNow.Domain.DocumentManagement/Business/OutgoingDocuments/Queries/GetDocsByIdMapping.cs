using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data.OutgoingDocuments;

namespace DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Queries
{
    public class GetDocsByIdMapping : Profile
    {
        public GetDocsByIdMapping()
        {
            CreateMap<OutgoingDocument, GetDocsByIdResponse>();
        }
    }
}
