using System.Net;
using FluentValidation.Results;

namespace People.Application.Exceptions;

public class ValidationException : CustomException
{
    public ValidationException(
        List<string>? errors = default,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest) :
        base("One or more validation failures have occurred.", errors, statusCode, Models.ResponseCode.Validation)
    {
    }

    public ValidationException(IEnumerable<ValidationFailure> failures) : this()
    {
        ErrorMessages = (from item in failures.GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray())
            from error in item.Value
            where error != null
            select error).ToList();
    }
}