using Microsoft.AspNetCore.Builder;
namespace NotesWebApi.Infrastructure.Middlewares;

public static class ExceptionHandlerMiddlewareExtensions
{
    public static void UseExceptionHandlerMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlerMiddleware>();
    }
}

