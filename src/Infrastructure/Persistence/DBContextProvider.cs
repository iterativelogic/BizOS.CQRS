using System.Collections.Generic;
using Common.Application.Contracts;
using Common.Application.Contracts.Persistance;
using Common.Domain.Entities;
using Infrastructure.Contracts;

namespace Infrastructure.Persistence
{
  public class DBContextProvider : IDBContextProvider
  {
    protected readonly IConnectionProvider connectionProvider;
    protected readonly IMetadataProvider metadataProvider;
    protected readonly IModelBuilder modelBuilder;

    private Dictionary<string, object> DbSetDictionary { get; }

    public DBContextProvider(
      IConnectionProvider connectionProvider,
      IMetadataProvider metadataProvider,
      IModelBuilder modelBuilder)
    {
      this.connectionProvider = connectionProvider;
      this.metadataProvider = metadataProvider;
      this.modelBuilder = modelBuilder;
      DbSetDictionary = new Dictionary<string, object>();
    }

    private TableSchema<T> GetTableSchema<T>()
    {
      var entitySchema = modelBuilder.Entity<T>() as EntitySchema<T>;
      return entitySchema.BuildSchema(metadataProvider);
    }

    public IDBSet<T> GetDBSet<T>() where T : Entity
    {
      var type = typeof(T);
      if (!DbSetDictionary.TryGetValue(type.FullName, out object dbSet))
      {
        var tableSchema = GetTableSchema<T>();
        dbSet = new DBSet<T>(connectionProvider, metadataProvider, tableSchema);
        DbSetDictionary.TryAdd(type.FullName, dbSet);
      }
      return dbSet as DBSet<T>;
    }

    public ITenantDBSet<T> GetTenantDBSet<T>() where T : TenantEntity
    {
      var type = typeof(T);
      if (!DbSetDictionary.TryGetValue(type.FullName, out object tenantDBSet))
      {
        var tableSchema = GetTableSchema<T>();
        tenantDBSet = new TenantDBSet<T>(connectionProvider, metadataProvider, tableSchema);
        DbSetDictionary.TryAdd(type.FullName, tenantDBSet);
      }
      return tenantDBSet as TenantDBSet<T>;
    }
  }
}
