namespace MessengerApp.Application.Errors;

public class Error
{
    public string Message { get; set; }
    public int Code { get; set; }

    
    public static Error Create(string message) =>
        new Error {Message = message};
    
    public static Error Create(string message, int code) =>
        new Error {Message = message, Code = code};
}