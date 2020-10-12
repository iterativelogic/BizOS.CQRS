using System.Transactions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Common.Middleware
{
  public class TransactionScopeMiddleware
  {
    private readonly RequestDelegate next;
    public TransactionScopeMiddleware(RequestDelegate next)
    {
      this.next = next;
    }

    public async Task Invoke(HttpContext context /* other dependencies */)
    {
      using(var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
      {
        await next(context);
        transactionScope.Complete();
      }
    }
  }
}
