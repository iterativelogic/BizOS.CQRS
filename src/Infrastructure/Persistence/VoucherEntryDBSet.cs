using System;
using System.Collections.Generic;
using System.Linq;
using Accounts.Domain.Entities;
using Common.Application.Contracts.Persistance;
using Infrastructure.Contracts;
using SqlKata;

namespace Infrastructure.Persistence
{
  public class VoucherEntryDBSet : TenantDBSet<VoucherEntry>
  {
    private readonly string voucherEntryIdColumnName;
    private readonly string voucherDetailVoucherIdColumnName;
    private readonly string voucherEntryTenantIdColumnName;
    private readonly string voucherDetailTenantIdColumnName;
    private readonly TableSchema<VoucherDetail> voucherDetailSchema;
    private readonly Type[] childTypes = new Type[] { typeof(VoucherEntry), typeof(VoucherDetail) };

    protected override Type[] ChildTypes => childTypes;

    protected override string splitOn => metadataProvider.GetColumnName(nameof(VoucherEntry.Description), string.Empty);

    public VoucherEntryDBSet(
      IConnectionProvider connectionProvider, 
      IMetadataProvider metadataProvider, 
      TableSchema<VoucherEntry> tableSchema, TableSchema<VoucherDetail> voucherDetailSchema) : base(connectionProvider, metadataProvider, tableSchema)
    {
      voucherEntryIdColumnName = metadataProvider.GetColumnName(nameof(VoucherEntry.Id), nameof(VoucherEntry));
      voucherEntryTenantIdColumnName = metadataProvider.GetColumnName(Constants.TenantIdFieldName, nameof(VoucherEntry));

      voucherDetailVoucherIdColumnName = metadataProvider.GetColumnName(nameof(VoucherDetail.VoucherId), nameof(VoucherDetail));
      voucherDetailTenantIdColumnName = metadataProvider.GetColumnName(Constants.TenantIdFieldName, nameof(VoucherDetail));

      this.voucherDetailSchema = voucherDetailSchema;
    }

    public override Query Query => new Query(tableSchema.TableName)
                  .Join(voucherDetailSchema.TableName,
                          j => j.On(voucherEntryIdColumnName, voucherDetailVoucherIdColumnName)
                                .On(voucherEntryTenantIdColumnName, voucherDetailTenantIdColumnName))
                  .Select(tableSchema.Columns.ToArray())
                  .Select(voucherDetailSchema.Columns.ToArray());

    protected override Func<object[], VoucherEntry> GetMapper()
    {
      var voucherDictionary = new Dictionary<Guid, VoucherEntry>();

      return (objects) =>
      {
        if (objects[0] is VoucherEntry voucherEntry && objects[1] is VoucherDetail voucherDetail)
        {
          if (!voucherDictionary.TryGetValue(voucherEntry.Id, out VoucherEntry voucher))
          {
            voucher = voucherEntry;
            voucher.VoucherDetails = new List<VoucherDetail>();
            voucherDictionary.Add(voucher.Id, voucher);
          }
          voucher.VoucherDetails.Add(voucherDetail);
          return voucherEntry;
        }
        return null;
      };
    }

    public override List<Query> GetInsertQuery(Guid tenantId, params VoucherEntry[] entities)
    {
      var voucherEntryQuery= new Query(tableSchema.TableName);
      var voucherDetailQuery = new Query(voucherDetailSchema.TableName);

      var voucherEntryDestructuredObjects = entities.Select(model => tableSchema.InsertPart(model)).ToList();
      voucherEntryDestructuredObjects.ForEach(objectArr => objectArr.Insert(0, tenantId));
      voucherEntryQuery.AsInsert(
        tableSchema.InsertColumns,
        voucherEntryDestructuredObjects
        );

      var voucherDetailDestructuredObjects = entities
                                              .SelectMany(voucherEntry => voucherEntry
                                                                             .VoucherDetails
                                                                               .Select(voucherDetail => voucherDetailSchema.InsertPart(voucherDetail)))
                                              .ToList();
      voucherDetailDestructuredObjects.ForEach(objectArr => objectArr.Insert(0, tenantId));
      voucherDetailQuery.AsInsert(
        voucherDetailSchema.InsertColumns,
        voucherDetailDestructuredObjects
        );

      return new List<Query> { voucherEntryQuery, voucherDetailQuery };
    }

    public override List<Query> GetDeleteQuery(Guid tenantId, params VoucherEntry[] entities)
    {
      var deleteQueries = entities
           .SelectMany(model => {
             var queries = new List<Query> 
             {
               new Query(tableSchema.TableName)
                              .Where(tableSchema.WherePart(model))
                              .AsDelete(),
               new Query(voucherDetailSchema.TableName)
                              .Where(voucherDetailVoucherIdColumnName, model.Id)
                              .AsDelete()
             };
             return queries;
            })
           .ToList();

      AppendTenantIdCondition(tenantId, deleteQueries);
      return deleteQueries;
    }

    public override List<Query> GetUpdateQuery(Guid tenantId, params VoucherEntry[] entities)
    {
      var deleteQueries = GetDeleteQuery(tenantId, entities);
      var insertQueries = GetInsertQuery(tenantId, entities);
      deleteQueries.AddRange(insertQueries);
      return deleteQueries;
    }
  }
}
