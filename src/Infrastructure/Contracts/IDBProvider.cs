using System.Data;
using System.Threading;
using System.Threading.Tasks;
using SqlKata.Compilers;

namespace Infrastructure.Contracts
{
  public interface IDBProvider
  {
    Compiler GetQueryCompiler();

    Task<IDbConnection> GetConnectionAsync(CancellationToken cancellationToken);

    Task<IDbConnection> GetConnectionAsync(DatabaseOptions databaseOptions, CancellationToken cancellationToken);
  }
}
