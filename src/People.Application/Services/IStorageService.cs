namespace People.Application.Services;

public interface IStorageService
{
    Task<bool> ExistsAsync(string container, string filename, CancellationToken cancellationToken = default);
    Task<byte[]> ReadAsync(string container, string filename, CancellationToken cancellationToken = default);
    Task DeleteAsync(string container, string filename, CancellationToken cancellationToken = default);
    Task WriteAsync(string container, string filename, byte[] data, CancellationToken cancellationToken = default);
    Task WriteAsync(string container, string filename, Stream stream, CancellationToken cancellationToken = default);
}