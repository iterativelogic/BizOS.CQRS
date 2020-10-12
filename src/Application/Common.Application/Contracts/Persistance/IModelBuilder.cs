namespace Common.Application.Contracts.Persistance
{
  public interface IModelBuilder
  {
    IEntitySchema<T> Entity<T>();

  }
}