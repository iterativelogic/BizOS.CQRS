
using Common.Application.Metadata;

namespace Common.Application.Contracts
{
  public interface IEntitySchemaContainer
  {
    void Register<T>(EntitySchema entitySchema);
  }
}
