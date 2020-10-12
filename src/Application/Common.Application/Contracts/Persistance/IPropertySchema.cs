namespace Common.Application.Contracts.Persistance
{
  public interface IPropertySchema
  {
    IPropertySchema Ignore();

    IPropertySchema IgnoreOnInsert();

    IPropertySchema IgnoreOnUpdate();

    IPropertySchema IsSearchColumn();

    IPropertySchema IsId();
  }
}