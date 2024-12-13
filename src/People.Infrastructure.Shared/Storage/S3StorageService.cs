using Amazon.S3;
using Amazon.S3.Model;
using People.Application.Services;

namespace People.Infrastructure.Shared.Storage;

public class S3StorageService : IStorageService
{
    private readonly IAmazonS3 _s3Client;

    public S3StorageService(IAmazonS3 s3Client)
    {
        _s3Client = s3Client;
    }
    
    public async Task<bool> ExistsAsync(string container, string filename, CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new GetObjectMetadataRequest
            {
                BucketName = container,
                Key = filename
            };

            var response = await _s3Client.GetObjectMetadataAsync(request, cancellationToken);
            return response != null;
        }
        catch (AmazonS3Exception ex) when (ex.ErrorCode == "NoSuchKey")
        {
            return false; // No exist file
        }
    }
    
    public async Task<byte[]> ReadAsync(string container, string filename, CancellationToken cancellationToken = default)
    {
        var request = new GetObjectRequest
        {
            BucketName = container,
            Key = filename
        };

        using var response = await _s3Client.GetObjectAsync(request, cancellationToken);
        using var memoryStream = new MemoryStream();
        await response.ResponseStream.CopyToAsync(memoryStream, cancellationToken);
        return memoryStream.ToArray();
    }
    
    public async Task DeleteAsync(string container, string filename, CancellationToken cancellationToken = default)
    {
        var request = new DeleteObjectRequest
        {
            BucketName = container,
            Key = filename
        };

        await _s3Client.DeleteObjectAsync(request, cancellationToken);
    }
    
    public async Task WriteAsync(string container, string filename, byte[] data, CancellationToken cancellationToken = default)
    {
        using var stream = new MemoryStream(data);
        await WriteAsync(container, filename, stream, cancellationToken);
    }
    
    public async Task WriteAsync(string container, string filename, Stream stream, CancellationToken cancellationToken = default)
    {
        var request = new PutObjectRequest
        {
            BucketName = container,
            Key = filename,
            InputStream = stream
        };

        await _s3Client.PutObjectAsync(request, cancellationToken);
    }
}
