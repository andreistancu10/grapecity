using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Documents.Models;
using DigitNow.Domain.DocumentManagement.Business.Documents.Commands.SetDocumentsResolution;
using DigitNow.Domain.DocumentManagement.Public.Documents.Models;
using System.Collections.Generic;

namespace DigitNow.Domain.DocumentManagement.Public.Documents.Mappings
{
    public class DocumentMappings : Profile
    {
        public DocumentMappings()
        {
            CreateMap<SetDocumentsResolutionRequest, SetDocumentsResolutionQuery>()
                .ForMember(dest => dest.Batch, opt => opt.MapFrom<DocumentBatchMapper>());
        }

        private class DocumentBatchMapper : IValueResolver<SetDocumentsResolutionRequest, SetDocumentsResolutionQuery, DocumentBatchModel>
        {
            public DocumentBatchModel Resolve(SetDocumentsResolutionRequest source, SetDocumentsResolutionQuery destination, DocumentBatchModel destMember, ResolutionContext context)
            {
                var documentBatchModel = new DocumentBatchModel
                {
                    Documents = new List<DocumentModel>()
                };

                foreach (var documentDto in source.Batch.Documents)
                {
                    documentBatchModel.Documents.Add(new DocumentModel
                    {
                        Id = documentDto.Id,
                        DocumentType = documentDto.DocumentType
                    });
                }

                return documentBatchModel;
            }
        }
    }
}
