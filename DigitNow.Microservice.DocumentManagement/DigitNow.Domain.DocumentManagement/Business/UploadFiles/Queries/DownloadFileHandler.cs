using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.SpecialRegisters.Queries;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.UploadFiles.Queries;

public class DownloadFileHandler : IQueryHandler<DownloadFileQuery, List<SpecialRegisterResponse>>
{
    private readonly IMapper _mapper;
    private readonly ISpecialRegisterService _specialRegisterService;

    public DownloadFileHandler(DocumentManagementDbContext dbContext,
        IMapper mapper,
        ISpecialRegisterService specialRegisterService)
    {
        _mapper = mapper;
        _specialRegisterService = specialRegisterService;
    }

    public async Task<List<SpecialRegisterResponse>> Handle(DownloadFileQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}