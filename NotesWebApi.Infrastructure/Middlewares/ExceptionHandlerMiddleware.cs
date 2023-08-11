﻿using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
namespace NotesWebApi.Infrastructure.Middlewares;
public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionMessageAsync(context, ex).ConfigureAwait(false);
        }
    }
    private static Task HandleExceptionMessageAsync(HttpContext context, Exception exception)
    {
        int statusCode;
        if (exception is HttpRequestException httpRequestException && httpRequestException.StatusCode != null)
        {
            statusCode = (int)(httpRequestException.StatusCode);
        }
        else
        {
            statusCode = (int)HttpStatusCode.InternalServerError;
        }
        var result = JsonSerializer.Serialize(new
        {
            ErrorMessage = exception.Message
        });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        return context.Response.WriteAsync(result);
    }
}