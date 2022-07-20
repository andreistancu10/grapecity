using System;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;

namespace DigitNow.Domain.DocumentManagement.Business.UploadFiles.Queries.GetFiles
{
    public class GetFilesResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime UploadDate { get; set; }
        public BasicViewModel Category { get; set; }
        public BasicViewModel User { get; set; }

        public GetFilesResponse()
        {
        }
    }
}