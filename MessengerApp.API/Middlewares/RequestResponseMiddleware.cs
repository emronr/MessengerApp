using System.ComponentModel.DataAnnotations;
using System.Net;
using MessengerApp.Application.Errors;
using MessengerApp.Application.Exception;
using MessengerApp.Application.RequestResponseModels.ResponseModels;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.IO;
using Newtonsoft.Json;

namespace MessengerApp.API.Middlewares;

public class RequestResponseMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
    private readonly RecyclableMemoryStreamManager _streamManager;

    public RequestResponseMiddleware(RequestDelegate next,
        ILoggerFactory loggerFactory)
    {
        this._next = next;
        this._logger = loggerFactory
            .CreateLogger<RequestResponseMiddleware>();
        this._streamManager = new RecyclableMemoryStreamManager();
    }

    public async Task Invoke(HttpContext context)
    {
        var requestMessage = await LogRequest(context);
        var originalBodyStream = context.Response.Body;
        var responseStream = _streamManager.GetStream();
        context.Response.Body = responseStream;

        Exception exception = null;
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            exception = ex;

            var responseBody = HandleException(context, ex);

            context.Response.WriteAsync(responseBody);
        }

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var text = await new StreamReader(context.Response.Body).ReadToEndAsync();
        context.Response.Body.Seek(0, SeekOrigin.Begin);

        var responseMessage = $"#### Http Response Information:" +
                              Environment.NewLine +
                              $"Schema:{context.Request.Scheme} " +
                              Environment.NewLine +
                              $"Host: {context.Request.Host} " +
                              Environment.NewLine +
                              $"Path: {context.Request.Path} " +
                              Environment.NewLine +
                              $"QueryString: {context.Request.QueryString} " +
                              Environment.NewLine +
                              $"Response Body: {text}";

        var message =
            Environment.NewLine + Environment.NewLine
                                + "*****************************************************"
                                + Environment.NewLine
                                + $"RequestId: {context.TraceIdentifier}"
                                + $"{Environment.NewLine}"
                                + $"{requestMessage}"
                                + $"{Environment.NewLine}"
                                + (exception == null
                                    ? null
                                    : "#### Exception:" + exception.Message + Environment.NewLine)
                                + $"{responseMessage}"
                                + $"{Environment.NewLine}"
                                + "*****************************************************"
                                + Environment.NewLine
                                + Environment.NewLine;


        await responseStream.CopyToAsync(originalBodyStream);

        if (exception == null)
            _logger.LogInformation(message);
        else
            _logger.LogError(message);
    }

    private string HandleException(HttpContext context, Exception exception)
    {
        context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/json";

        string responseBody = string.Empty;
        var error = new Error()
        {
            Message = HttpStatusCode.InternalServerError.ToString()
        };

        var response = new BaseResponse(error, HttpStatusCode.InternalServerError);

        if (exception is UserFriendlyException userFriendlyException)
        {
            context.Response.StatusCode = (int) userFriendlyException.HttpStatusCode;
            error = userFriendlyException.Error;
            
            response = new BaseResponse(error, userFriendlyException.HttpStatusCode);
        }
        else
        {
            error.Message = exception.Message;
            response = new BaseResponse(error);
        }

        responseBody = JsonConvert.SerializeObject(response);
        return responseBody;
    }

    private async Task<string> LogRequest(HttpContext context)
    {
        context.Request.EnableBuffering();
        var requestStream = _streamManager.GetStream();
        await context.Request.Body.CopyToAsync(requestStream);

        var message = $"###Http Request Information:" +
                      Environment.NewLine +
                      $"Schema: {context.Request.Scheme} " +
                      Environment.NewLine +
                      $"Host: {context.Request.Host} " +
                      Environment.NewLine +
                      $"Path: {context.Request.Path} " +
                      Environment.NewLine +
                      $"QueryString: {context.Request.QueryString} " +
                      Environment.NewLine +
                      $"Response Body: {ReadStreamInChunks(requestStream)} " +
                      Environment.NewLine +
                      $"Header Key: Authorization - Value: {context.Request.Headers["Authorization"]}";

        context.Request.Body.Position = 0;

        return message;
    }

    private static string ReadStreamInChunks(Stream stream)
    {
        const int readChunkBufferLenght = 4096;
        stream.Seek(0, SeekOrigin.Begin);
        var textWriter = new StringWriter();
        var reader = new StreamReader(stream);
        var readChunk = new char[readChunkBufferLenght];
        int readChunkLength;
        do
        {
            readChunkLength = reader.ReadBlock(readChunk,
                0,
                readChunkBufferLenght);
            textWriter.Write(readChunk, 0, readChunkLength);
        } while (readChunkLength > 0);

        return textWriter.ToString();
    }
}