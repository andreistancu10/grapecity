using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Export;
using DigitNow.Domain.DocumentManagement.Data.Filters.Documents;
using DigitNow.Domain.DocumentManagement.utils;
using HTSS.Platform.Core.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Dashboard.Queries.OperationalDocumentArchiveExport
{
    public class OperationalDocumentArchiveExportQuery : IQuery<List<ExportDocumentViewModel>>
    {
        public int LanguageId { get; set; } = LanguagesUtils.RomanianLanguageId;
        public DocumentFilter Filter { get; set; }
    }

}