using Common.Application.Contracts;
using Common.Application.Contracts.Persistance;
using Common.Domain.Entities;

namespace Infrastructure.Persistence
{
  public class CommonDBContext : ICommonDBContext
  {
    private readonly IDBContextProvider dbContextProvider;

    private IDBSet<City> cities;
    private IDBSet<State> states;
    private IDBSet<Country> countries;

    public CommonDBContext(IDBContextProvider dbContextProvider)
    {
      this.dbContextProvider = dbContextProvider;
    }

    public IDBSet<City> Cities => cities ??= dbContextProvider.GetDBSet<City>();

    public IDBSet<State> States => states ??= dbContextProvider.GetDBSet<State>();

    public IDBSet<Country> Countries => countries ??= dbContextProvider.GetDBSet<Country>();
  }
}
