using System.Net;
using MessengerApp.Application.Errors;
using MessengerApp.Domain.Common;

namespace MessengerApp.Application.RequestResponseModels.ResponseModels;

public class BaseResponse
{
    public int? HttpStatusCode { get; set; }

    public List<Error> Errors { get; set; } = new List<Error>();

    public string Message { get; set; }

    public bool isSuccess { get; set; }

    public BaseResponse()
    {
        Errors = new List<Error>();
    }

    public BaseResponse(Error error, HttpStatusCode statusCode = System.Net.HttpStatusCode.BadRequest)
    {
        Errors.Add(error);
        isSuccess = false;
        HttpStatusCode = (int) statusCode;
    }

}

public class BaseResponse<T> : BaseResponse
{
    public T Result { get; set; }

    public BaseResponse() : base()
    {
    }
    
    public BaseResponse(Error error, HttpStatusCode statusCode = System.Net.HttpStatusCode.BadRequest) : base(error, statusCode)
    {
    }
    
    public BaseResponse(T result, HttpStatusCode statusCode = System.Net.HttpStatusCode.OK) 
    {
        Result = result;
        isSuccess = Errors.Any() ? false : true;
        HttpStatusCode = (int) statusCode;
    }
    
}