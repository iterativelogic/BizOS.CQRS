using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Accounts.Application.Contracts.Persistance;
using Accounts.Domain.Entities;
using Common.Application.Contracts.Commands;
using SqlKata;

namespace Accounts.Application.Queries.Voucher.Handlers
{
  public class GetVouchersHandler : AbstractQueryHandler<GetVouchersQuery, IEnumerable<VoucherEntry>>
  {
    private readonly IAccountDBContext accountDBContext;

    public GetVouchersHandler(
      IAccountDBContext accountDBContext
      )
    {
      this.accountDBContext = accountDBContext;
    }

    public override async Task<IEnumerable<VoucherEntry>> PerformQueryAsync(GetVouchersQuery request, CancellationToken cancellationToken)
    {
      var query = accountDBContext.VoucherEntries.Query;
      AppendWhereConditions(request, query);
      return await accountDBContext.VoucherEntries.QueryAsync(request.TenantId, query, cancellationToken);
    }

    private void AppendWhereConditions(GetVouchersQuery request, Query query)
    {
      AppendVoucherIdCondition(request, query);
      AppendVoucherTypeCondition(request, query);
      AppendDateRangeCondition(request, query);
    }

    private void AppendDateRangeCondition(GetVouchersQuery request, Query query)
    {
      if (request.Range != null)
      {
        query.WhereBetween(accountDBContext.VoucherEntries.GetColumnName(nameof(VoucherEntry.Date)), request.Range.From, request.Range.To);
      }
    }

    private void AppendVoucherTypeCondition(GetVouchersQuery request, Query query)
    {
      if (request.VoucherType.HasValue)
      {
        query.Where(accountDBContext.VoucherEntries.GetColumnName(nameof(VoucherEntry.Type)), request.VoucherType);
      }
    }

    private void AppendVoucherIdCondition(GetVouchersQuery request, Query query)
    {
      if (request.VoucherId.HasValue)
      {
        query.Where(accountDBContext.VoucherEntries.GetColumnName(nameof(VoucherEntry.Id)), request.VoucherId);
      }
    }
  }
}