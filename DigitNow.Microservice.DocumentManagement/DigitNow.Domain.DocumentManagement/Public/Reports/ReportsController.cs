﻿using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Business.Dashboard.Queries;
using HTSS.Platform.Core.Files;
using HTSS.Platform.Infrastructure.Api.Tools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitNow.Domain.DocumentManagement.Public.Reports;

[Authorize]
[ApiController]
[Route("api/reports")]
public class ReportsController : ApiController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IIdentityService _identityService;
    private readonly IExportService<GetDocumentResponse> _exportService;

    public ReportsController(IMediator mediator, IMapper mapper, IIdentityService identityService, IExportService<GetDocumentResponse> exportService)
    {
        _mediator = mediator;        
        _mapper = mapper;
        _identityService = identityService;
        _exportService = exportService;
    }
    
}