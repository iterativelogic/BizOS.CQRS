using System.Linq;
using Accounts.Domain.Entities;

namespace Accounts.Application.Commands.Voucher.Validations
{
  public class VoucherValidations : IVoucherValidations
  {
    public bool IsTotalMatch(VoucherEntry voucherEntry)
    {
      var creditTotal = voucherEntry.VoucherDetails.Sum(voucherDetail => voucherDetail.Credit);
      var debitTotal = voucherEntry.VoucherDetails.Sum(voucherDetail => voucherDetail.Debit);
      return creditTotal == debitTotal;
    }
  }
}
