using Common.Domain.Entities;

namespace Common.Application.Contracts.Persistance
{
  public interface IDBContext
  {
    IDBSet<T> GetDBSet<T>() where T : Entity;

    ITenantDBSet<T> GetTenantDBSet<T>() where T : TenantEntity;
  }
}
