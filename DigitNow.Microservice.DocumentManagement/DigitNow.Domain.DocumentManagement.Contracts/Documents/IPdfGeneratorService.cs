using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Contracts.Documents
{
    public interface IPdfGeneratorService
    {
        public Task<FileContent> GeneratePdfAsync(string filePath, string pdfName, List<Tuple<string, string>> replaceableStrings);
    }
}
