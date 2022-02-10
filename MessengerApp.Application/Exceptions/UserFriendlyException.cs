using System.Net;
using MessengerApp.Application.Errors;

namespace MessengerApp.Application.Exception;

public class UserFriendlyException : System.Exception
{
    public HttpStatusCode HttpStatusCode { get; set; }
    public Error Error = new();

    public UserFriendlyException(string errorMessage, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
        : base(errorMessage)
    {
        HttpStatusCode = httpStatusCode;
        Error.Code = (int) httpStatusCode;
        Error.Message = errorMessage;
    }
    
    public UserFriendlyException(string errorMessage, int errorCode, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
        : base(errorMessage)
    {
        HttpStatusCode = httpStatusCode;
        Error.Code = errorCode;
        Error.Message = errorMessage;
    }
}