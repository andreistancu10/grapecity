using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.OperationalArchive.Commands.Update.MoveDocumentsToArchiveByIds;
using DigitNow.Domain.DocumentManagement.Public.Archive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Public.Archive.Mappings
{
    public class MoveDocumentsToArchiveByIdsMapping : Profile
    {
        public MoveDocumentsToArchiveByIdsMapping()
        {
            CreateMap<MoveDocumentsToArchiveByIdsRequest, MoveDocumentsToArchiveByIdsCommand>();
        }
    }
}
