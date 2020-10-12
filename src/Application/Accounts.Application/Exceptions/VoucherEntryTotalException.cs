using System;
using System.Runtime.Serialization;

namespace Accounts.Application.Exceptions
{
  [Serializable]
  [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly", Justification = "Not required for VoucherEntryTotalException")]
  public class VoucherEntryTotalException : VoucherEntryException
  {
    const string Exception_Message = "Voucher debit and credit total must match";

    public VoucherEntryTotalException() : base(Exception_Message)
    {}

    public VoucherEntryTotalException(Exception innerException) : base(Exception_Message, innerException)
    {}

    protected VoucherEntryTotalException(SerializationInfo serializationInfo, StreamingContext streamingContext)
    {
      throw new NotImplementedException();
    }
  }
}
