
using Common.Domain.Entities;

namespace Common.Application.Contracts.Persistance
{
  public interface ICommonDBContext
  {

    IDBSet<City> Cities { get; }

    IDBSet<State> States { get; }

    IDBSet<Country> Countries { get; }
  }
}
