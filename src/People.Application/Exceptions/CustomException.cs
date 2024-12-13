using System.Net;

namespace People.Application.Exceptions;

public class CustomException : Exception
{
    public List<string>? ErrorMessages { get; protected set; }

    public HttpStatusCode StatusCode { get; }
    
    public string ResponseCode { get; }

    public CustomException(
        string message, 
        List<string>? errors = default, 
        HttpStatusCode statusCode = HttpStatusCode.BadRequest, 
        string responseCode = "Unhandled")
        : base(message)
    {
        ErrorMessages = errors;
        StatusCode = statusCode;
        ResponseCode = responseCode;
    }
}