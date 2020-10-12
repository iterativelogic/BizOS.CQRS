
using Common.Application.Contracts.Persistance;
using Common.Domain.Entities;

namespace Common.Application.Contracts
{
  public interface IDBContextProvider
  {
    IDBSet<T> GetDBSet<T>() where T : Entity;

    ITenantDBSet<T> GetTenantDBSet<T>() where T : TenantEntity;

  }
}
