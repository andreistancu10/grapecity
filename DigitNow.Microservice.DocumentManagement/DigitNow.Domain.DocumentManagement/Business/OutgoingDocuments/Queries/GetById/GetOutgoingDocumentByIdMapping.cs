using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Queries.GetById
{
    public class GetOutgoingDocumentByIdMapping : Profile
    {
        public GetOutgoingDocumentByIdMapping()
        {
            CreateMap<OutgoingDocument, GetOutgoingDocumentByIdResponse>();
        }
    }
}