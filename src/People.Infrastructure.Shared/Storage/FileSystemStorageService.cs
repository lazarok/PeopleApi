using People.Application.Helpers;
using People.Application.Services;

namespace People.Infrastructure.Shared.Storage;

public class FileSystemStorageService : IStorageService
{
    private readonly string _path;

    public FileSystemStorageService(string path)
    {
        _path = path;
    }

    public async Task<bool> ExistsAsync(string container, string filename, CancellationToken cancellationToken = default)
    {
        var exists = File.Exists(GetPath(container, filename));
        return await Task.FromResult(exists);
    }

    public async Task<byte[]> ReadAsync(string container, string filename, CancellationToken cancellationToken = default)
    {
        var bytes = await File.ReadAllBytesAsync(GetPath(container, filename), cancellationToken);
        return bytes;
    }

    public Task DeleteAsync(string container, string filename, CancellationToken cancellationToken = default)
    {
        File.Delete(GetPath(container, filename));
        return Task.CompletedTask;
    }

    public async Task WriteAsync(string container, string filename, byte[] data, CancellationToken cancellationToken = default)
    {
        await File.WriteAllBytesAsync(GetPath(container, filename), data, cancellationToken);
    }
    
    public async Task WriteAsync(string container, string filename, Stream stream, CancellationToken cancellationToken = default)
    {
        await File.WriteAllBytesAsync(GetPath(container, filename), FileHelpers.ToBytes(stream), cancellationToken);
    }

    private string GetPath(string container, string filename)
    {
        if (!Directory.Exists(_path))
        {
            Directory.CreateDirectory(_path);
        }

        var dir = Path.Combine(_path, container);
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        return Path.Combine(dir, filename);
    }
}