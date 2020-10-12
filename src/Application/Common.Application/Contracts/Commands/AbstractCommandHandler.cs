using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Common.Application.Contracts.Commands
{
  public abstract class AbstractCommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, TResponse> where TCommand : IRequest<TResponse>
  {
    public abstract Task<TResponse> HandleAsync(TCommand request, CancellationToken cancellationToken);

    public async Task<TResponse> Handle(TCommand request, CancellationToken cancellationToken)
    {
      //Perform common actions here
      return await HandleAsync(request, cancellationToken);
    }
  }
}
