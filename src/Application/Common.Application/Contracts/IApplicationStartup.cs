
using Common.Application.Contracts.Persistance;

namespace Common.Application.Contracts 
{
  public interface IApplicationStartup
  {
    void RegisterEntitySchema(IEntitySchemaContainer container);

    void OnApplicationStart(IModelBuilder modelBuilder);
  }
}