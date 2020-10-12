using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BizOS.Account.Middelware
{
  public class ErrorHandlingMiddleware
  {
    private readonly RequestDelegate next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
      this.next = next;
    }

    public async Task Invoke(HttpContext context /* other dependencies */)
    {
      try
      {
        await next(context);
      }
      catch (Exception ex)
      {
        await HandleExceptionAsync(context, ex);
      }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
      var code = HttpStatusCode.InternalServerError; // 500 if unexpected
      var response = new Response<string>
      {
        ResponseCode = HttpStatusCode.InternalServerError,
        Messages = new List<Message>() {
              new Message (
                 MessageType.Error,
                $"{ex.Message}  {Environment.NewLine} {ex.StackTrace} {Environment.NewLine} {ex.InnerException?.Message}"
              )
            }
      };
      context.Response.ContentType = "application/json";
      context.Response.StatusCode = (int)code;
      return context.Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings
      {
        ContractResolver = new CamelCasePropertyNamesContractResolver()
      }));
    }
  }
}
