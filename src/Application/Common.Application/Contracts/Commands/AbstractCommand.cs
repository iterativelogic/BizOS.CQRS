using MediatR;

namespace Common.Application.Contracts.Commands
{
  public abstract class AbstractCommand<TModel> : IRequest<TModel>
  {

  }
}
