using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Infrastructure.Models;
using Infrastructure.Validations;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Infrastructure.Middleware
{
  public class ErrorHandlingMiddleware
  {
    private readonly RequestDelegate next;
    private readonly JsonSerializerSettings settings;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
      this.next = next;
      settings = new JsonSerializerSettings
      {
        ContractResolver = new CamelCasePropertyNamesContractResolver()
      };
    }

    public async Task Invoke(HttpContext context /* other dependencies */)
    {
      try
      {
        await next(context);
      }
      catch (CommandValidationException ex)
      {
        await HandleExceptionAsync(context, ex);
      }
      catch (Exception ex)
      {
        await HandleExceptionAsync(context, ex);
      }
    }

    private async Task HandleExceptionAsync(HttpContext context, CommandValidationException ex)
    {
      var code = HttpStatusCode.BadRequest; // 400 if unexpected
      var response = new Response<string>
      {
        ResponseCode = code,
        Messages = ex.ErrorCodes
                      .Select(error => new Message(MessageType.Validation, error.Code, error.Description))
                      .ToList()
      };

      await WriteToResponse(context, code, response);
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
      var code = HttpStatusCode.InternalServerError; // 500 if unexpected
      var response = new Response<string>
      {
        ResponseCode = HttpStatusCode.InternalServerError,
        Messages = new List<Message>() {
              new Message (
                 MessageType.Error,
                 "INTERNAL_SERVER_ERROR",
                $"{ex.Message}  {System.Environment.NewLine} {ex.StackTrace} {System.Environment.NewLine} {ex.InnerException?.Message}"
              )
            }
      };

      await WriteToResponse(context, code, response);
    }

    private async Task WriteToResponse(HttpContext context, HttpStatusCode code, Response<string> response)
    {
      context.Response.ContentType = "application/json";
      context.Response.StatusCode = (int)code;
      await context.Response.WriteAsync(JsonConvert.SerializeObject(response, settings));
    }
  }
}
