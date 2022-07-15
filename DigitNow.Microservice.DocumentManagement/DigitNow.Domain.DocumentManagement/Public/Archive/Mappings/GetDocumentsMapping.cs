using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Archive.Queries;
using DigitNow.Domain.DocumentManagement.Public.Archive.Models;

namespace DigitNow.Domain.DocumentManagement.Public.Archive.Mappings
{
    public class ArchiveMappings: Profile
    {
        public ArchiveMappings()
        {
            CreateMap<GetArchivedDocumentsRequest, GetArchivedDocumentsQuery>();
        }
    }
}
