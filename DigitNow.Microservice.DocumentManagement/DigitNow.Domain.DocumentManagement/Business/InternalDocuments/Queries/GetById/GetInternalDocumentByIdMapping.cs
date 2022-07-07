using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.InternalDocuments.Queries.GetById
{
    public class GetInternalDocumentByIdMapping : Profile
    {
        public GetInternalDocumentByIdMapping()
        {
            CreateMap<InternalDocument, GetInternalDocumentByIdResponse>();
        }
    }
}