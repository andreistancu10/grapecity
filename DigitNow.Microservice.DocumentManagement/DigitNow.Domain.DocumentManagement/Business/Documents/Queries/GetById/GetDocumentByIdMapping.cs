using AutoMapper;
using System.Reflection.Metadata;

namespace DigitNow.Domain.DocumentManagement.Business.Documents.Queries.GetById
{
    public class GetDocumentByIdMapping : Profile
    {
        public GetDocumentByIdMapping()
        {
            CreateMap<Document, GetDocumentByIdResponse>();
        }
    }
}
