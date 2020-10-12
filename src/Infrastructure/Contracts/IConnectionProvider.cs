
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using SqlKata.Compilers;

namespace Infrastructure.Contracts
{
  public interface IConnectionProvider
  {
    Task<IDbConnection> GetConnectionAsync(CancellationToken cancellationToken);

    Compiler QueryCompiler { get; }
  }
}
