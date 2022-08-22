using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.UploadFiles.Commands.Upload;
using DigitNow.Domain.DocumentManagement.Business.UploadFiles.Queries;
using DigitNow.Domain.DocumentManagement.Business.UploadFiles.Queries.DownloadFile;
using DigitNow.Domain.DocumentManagement.Business.UploadFiles.Queries.GetFiles;
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

        [HttpGet("get-files/document-usage/{documentId:int}")]
        public async Task<IActionResult> GetFilesAsync([FromRoute] int documentId)
        {
            var result = await _mediator.Send(new GetFilesForDocumentQuery(documentId));

            return result switch
            {
                null => NotFound(),
                _ => Ok(result)
            };
        }
    }
}