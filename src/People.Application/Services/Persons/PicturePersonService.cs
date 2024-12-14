using Microsoft.Extensions.Configuration;
using People.Application.Helpers;
using People.Application.Models;
using People.Domain.Entities;

namespace People.Application.Services.Persons;

public interface IPicturePersonService
{
    Task<string> UploadAsync(Person person, Stream content);
    Task<ApiResponse<MediaFileModel>> GetPictureAsync(string filename, CancellationToken cancellation = default);
    Task DeletePictureAsync(string filename);
}

public class PicturePersonService : IPicturePersonService
{
    private readonly IStorageService _storageService;
    private const string ContainerName = "Persons";

    public PicturePersonService(IStorageService storageService, IConfiguration configuration)
    {
        _storageService = storageService;
    }


    public async Task<string> UploadAsync(Person person, Stream content)
    {
        string filename = person.Picture ?? $"{Guid.NewGuid().ToString()}.jpg";
        await _storageService.WriteAsync(ContainerName, filename, content);
        return filename;
    }

    public async Task<ApiResponse<MediaFileModel>> GetPictureAsync(string filename, CancellationToken cancellation = default)
    {
        var bytes = await _storageService.ReadAsync(ContainerName, filename, cancellation);

        return ApiResponse.Ok(new MediaFileModel()
        {
            Filename = filename,
            FileContent = FileHelpers.ToStream(bytes)
        });
    }

    public async Task DeletePictureAsync(string filename)
    {
        await _storageService.DeleteAsync(ContainerName, filename);
    }
}