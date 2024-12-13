using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using People.Api.Controllers.Base;
using People.Application.Features.Persons.Commands.UploadAvatar;
using People.Application.Models;
using People.Application.Services.Persons;

namespace People.Api.Controllers;

[Route("api/mediafiles")]
public class MediaFileController : BaseApiController
{
    private readonly IPicturePersonService _picturePersonService;

    public MediaFileController(IPicturePersonService picturePersonService)
    {
        _picturePersonService = picturePersonService;
    }

    /// <summary>
    /// Upload personal picture
    /// </summary>
    /// <param name="personId"></param>
    /// <param name="file"></param>
    /// <returns></returns>
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [HttpPost("upload/person/{personId}")]
    [RequestSizeLimit(5 * 1024 * 1024)] // Max 5 MB
    [RequestFormLimits(MultipartBodyLengthLimit = 5 * 1024 * 1024)] // Max 5 MB
    public async Task<ActionResult> UploadImage([FromRoute] long personId, IFormFile? file)
    {
        if (file == null || file.Length == 0)
            return StatusCode(StatusCodes.Status400BadRequest,  ApiResponse.Error("File is empty"));
        
        var allowedMimeTypes = new[] { "image/jpeg", "image/png", "image/gif" };
        if (!allowedMimeTypes.Contains(file.ContentType.ToLower()))
            return StatusCode(StatusCodes.Status400BadRequest,  ApiResponse.Error("File only allows image."));
        
        await using var stream = file.OpenReadStream();

        var command = new UploadAvatarCommand(
            PersonId: personId,
            MediaFile: new MediaFileModel()
            {
                Filename = file.FileName,
                FileContent = stream
            }
        );
        
        return StatusCode(StatusCodes.Status200OK, await Mediator.Send(command));
    }
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("preview/{filename}")]
    public async Task<ActionResult> Preview(string filename)
    {
        var apiResponse = await _picturePersonService.GetPictureAsync(filename);
        if (!apiResponse.Success)
        {
            return BuildResponse(apiResponse);
        }

        if (apiResponse.Data == null)
        {
            return NotFound();
        }
        
        var provider = new FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(apiResponse.Data.Filename, out var contentType))
        {
            contentType = "application/octet-stream";
        }
            
        return File(apiResponse.Data.FileContent, contentType);
    }
}