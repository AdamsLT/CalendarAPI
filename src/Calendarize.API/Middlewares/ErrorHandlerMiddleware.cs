using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Calendarize.API.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                _logger.LogError(JsonConvert.SerializeObject(error));

                var response = context.Response;
                //Set response ContentType
                response.ContentType = "application/json";

                //Set custome error message for response model
                var responseContent = new ResponseContent()
                {
                    error = error.Message
                };
                //handler many Exception types
                switch (error)
                {
                    case ArgumentException:
                        response.StatusCode = StatusCodes.Status400BadRequest;
                        break;
                    default:
                        response.StatusCode = StatusCodes.Status500InternalServerError;
                        break;
                }
                //Using Newtonsoft.Json to convert object to json string
                var jsonResult = JsonConvert.SerializeObject(responseContent);
                await response.WriteAsync(jsonResult);
            }
        }
        //Response Model
        public class ResponseContent
        {
            public string error { get; set; }
        }
    }
}
