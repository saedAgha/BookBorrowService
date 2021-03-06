using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using BookBorrowService.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;
namespace BookBorrowService
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next.Invoke(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);

            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            int code;
            string errorMsg;
            GetStatusCode(exception, out code, out errorMsg);
            // GetStatusCode(exception);
            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = code;
 
            var problem = new ProblemDetails
            {
                Status = code,
                Title = errorMsg,

            };

            var traceId = Activity.Current?.Id ?? context?.TraceIdentifier;
            problem.Extensions["traceId"] = traceId;

            if (code >= 400)
            {
                _logger.LogError($"{exception} {getHttpContextDataAsString(context)} traceId: {traceId} status code:{code}");
            }

            //Serialize the problem details object to the Response as JSON (using System.Text.Json)
            var stream = context.Response.Body;
            await JsonSerializer.SerializeAsync(stream, problem);
        }

        private static void GetStatusCode(Exception ex ,out int statusCode,out string errorMsg )
        {
            if (ex is ArgumentException)
            {
                statusCode =(int) HttpStatusCode.BadRequest;
                errorMsg = ex.Message;
            } 
            else if (ex is NotFoundException)
            {
                statusCode = (int)HttpStatusCode.NotFound;
                errorMsg = ex.Message;

            }
            else
            {
                statusCode = (int)HttpStatusCode.InternalServerError;
                errorMsg = "Internal Server Error";
            }

        }

        private  string getHttpContextDataAsString(HttpContext httpContext)
        {
            if (httpContext.Request == null)
                return string.Empty;
            var request = httpContext.Request;
            return $" Request - path:{request.Path} ContentLength:{request.ContentLength} ContentType:{request.ContentType}" +
                   "QueryString:{request.QueryString}" +
                   $" PathBase:{request.PathBase} Method:{request.Method} Host:{request.Host} ";
        }

    }
}
