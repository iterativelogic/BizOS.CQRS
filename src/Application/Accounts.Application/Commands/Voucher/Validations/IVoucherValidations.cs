using Accounts.Domain.Entities;

namespace Accounts.Application.Commands.Voucher.Validations
{
  public interface IVoucherValidations
  {
    bool IsTotalMatch(VoucherEntry voucherEntry);
  }
}