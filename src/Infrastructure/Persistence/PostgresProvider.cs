using System;
using System.Data;
using Microsoft.Extensions.Options;
using Npgsql;
using SqlKata.Compilers;
using System.Threading.Tasks;
using System.Threading;
using Infrastructure.Contracts;

namespace Infrastructure.Persistence
{
  public class PostgresProvider : IDBProvider
  {
    private readonly Func<DatabaseOptions, string> ConnectionString =
   (options) => $"Server={options.Server};Port={options.Port ?? AppSettings.Postgres.Port};Database={options.Database};User Id={options.UserId};Password={options.Password};";

    private Compiler queryCompiler;

    private readonly IOptions<DatabaseOptions> _options;

    public PostgresProvider(IOptions<DatabaseOptions> options)
    {
      _options = options;
    }

    public Compiler GetQueryCompiler() => queryCompiler ??= new PostgresCompiler();

    public async Task<IDbConnection> GetConnectionAsync(CancellationToken cancellationToken) => await GetConnectionAsync(_options.Value, cancellationToken);

    public async Task<IDbConnection> GetConnectionAsync(DatabaseOptions databaseOptions, CancellationToken cancellationToken)
    {
      if (databaseOptions == null)
        throw new NoNullAllowedException("Invalid database options");

      var connection = new NpgsqlConnection(ConnectionString(databaseOptions));
      await connection.OpenAsync(cancellationToken);
      return connection;
    }
  }
}
