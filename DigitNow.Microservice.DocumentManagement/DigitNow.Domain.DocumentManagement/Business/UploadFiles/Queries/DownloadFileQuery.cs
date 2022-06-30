using System.Collections.Generic;
using DigitNow.Domain.DocumentManagement.Business.SpecialRegisters.Queries;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.UploadFiles.Queries;

public class DownloadFileQuery : IQuery<List<SpecialRegisterResponse>>
{
}