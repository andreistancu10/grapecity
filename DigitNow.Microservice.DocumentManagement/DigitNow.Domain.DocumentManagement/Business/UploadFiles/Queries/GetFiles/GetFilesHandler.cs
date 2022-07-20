using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Files.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.UploadFiles.Queries.GetFiles
{
    public class GetFilesHandler : IQueryHandler<GetFilesQuery, List<GetFilesResponse>>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public GetFilesHandler(
            DocumentManagementDbContext dbContext,
            IMapper mapper, IFileService fileService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<List<GetFilesResponse>> Handle(GetFilesQuery request, CancellationToken cancellationToken)
        {
            return null;
        }
    }
}