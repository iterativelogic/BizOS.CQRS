using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Contracts;
using SqlKata.Compilers;

namespace Infrastructure.Persistence
{
  public class ConnectionProvider : IConnectionProvider
  {
    private readonly IDBProvider dbProvider;

    public Compiler QueryCompiler { get => dbProvider.GetQueryCompiler(); }

    public ConnectionProvider(IDBProvider dBProvider)
    {
      this.dbProvider = dBProvider;
    }

    public Task<IDbConnection> GetConnectionAsync(CancellationToken cancellationToken)
    {
      return dbProvider.GetConnectionAsync(cancellationToken);
    }
  }
}
