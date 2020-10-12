
using Common.Application.Metadata;

namespace Common.Application.Contracts
{
  public interface IEntitySchemaProvider
  {
    public EntitySchema GetEntitySchema<T>();

    public void RegisterSchema<T>(EntitySchema entitySchema);
  }
}
