using MediatR;

namespace Common.Application.Contracts.Commands
{
  public class AbstractQuery<TResponse> : IRequest<TResponse>
  {
  }
}
