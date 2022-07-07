using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Queries.GetById
{
    public class GetIncomingDocumentByIdMapping : Profile
    {
        public GetIncomingDocumentByIdMapping()
        {
            CreateMap<IncomingDocument, GetIncomingDocumentByIdResponse>();
        }
    }
}