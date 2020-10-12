
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Common.Application.Contracts.Commands
{
  public abstract class AbstractQueryHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
  {
    public abstract Task<TResponse> PerformQueryAsync(TRequest request, CancellationToken cancellationToken);

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
    {
      return await PerformQueryAsync(request, cancellationToken);
    }
  }
}
