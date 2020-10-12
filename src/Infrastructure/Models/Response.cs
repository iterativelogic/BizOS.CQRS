using System.Collections.Generic;
using System.Net;

namespace Infrastructure.Models
{
  public class Response<T>
  {
    public T Data { get; set; }
    public HttpStatusCode ResponseCode { get; set; }
    public List<Message> Messages { get; set; }
    public bool Success { get; set; }

  }
}
