namespace People.Application.Models;

public static class ResponseCode
{
    public static string Ok { get; } = nameof(Ok);
    public static string Unhandled { get; } = nameof(Unhandled);
    public static string Unauthorized { get; } = nameof(Unauthorized);
    public static string Forbidden { get; } = nameof(Forbidden);
    public static string Validation { get; } = nameof(Validation);
    public static string RefreshTokenExpired { get; } = nameof(RefreshTokenExpired);
    public static string InvalidRefreshToken { get; } = nameof(InvalidRefreshToken);
    public static string NotFound { get; } = nameof(NotFound);
    public static string Found { get; } = nameof(Found);
    public static string Created { get; } = nameof(Created);
    public static string BadRequest { get; } = nameof(BadRequest);
}