using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.UploadFiles.Commands.Upload;
using DigitNow.Domain.DocumentManagement.Business.UploadFiles.Queries.DownloadFile;
using DigitNow.Domain.DocumentManagement.Business.UploadFiles.Queries.GetUploadedFilesForDocumentId;
using DigitNow.Domain.DocumentManagement.Business.UploadFiles.Queries.GetUploadedFilesForTargetId;
using DigitNow.Domain.DocumentManagement.Contracts.UploadedFiles.Enums;
using DigitNow.Domain.DocumentManagement.Public.UploadFiles.Models;
using HTSS.Platform.Infrastructure.Api.Tools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitNow.Domain.DocumentManagement.Public.UploadFiles
{
    [Authorize]
    [ApiController]
    [Route("api/upload-file")]
    public class UploadFilesController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public UploadFilesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadAsync([FromForm] UploadFileRequest request)
        {
            if ((TargetEntity)request.TargetEntity == TargetEntity.Document)
            {
                return await _mediator.Send(_mapper.Map<UploadDocumentFileCommand>(request))
                switch
                {
                    null => NotFound(),
                    var result => Ok(result)
                };
            }

            return await _mediator.Send(_mapper.Map<UploadFileCommand>(request))
            switch
            {
                null => NotFound(),
                var result => Ok(result)
            };
        }

        [HttpGet("download/{fileId:int}")]
        public async Task<IActionResult> DownloadAsync([FromRoute] int fileId)
        {
            var result = await _mediator.Send(new DownloadFileQuery(fileId));

            return result switch
            {
                null => NotFound(),
                _ => File(result.FileContent.Content, result.FileContent.ContentType, result.FileContent.Name)
            };
        }

        [HttpGet("get-files/{targetEntity}/{entityId:int}")]
        public async Task<IActionResult> GetFilesAsync([FromRoute] TargetEntity targetEntity, [FromRoute] int entityId)
        {

            if (targetEntity == TargetEntity.Document)
            {
                return await _mediator.Send(new GetUploadedFilesForDocumentIdQuery(entityId))
                switch
                {
                    null => NotFound(),
                    var result => Ok(result)
                };
            }

            return await _mediator.Send(new GetUploadedFilesForTargetIdQuery(targetEntity, entityId))
            switch
            {
                null => NotFound(),
                var result => Ok(result)
            };
        }
    }
}