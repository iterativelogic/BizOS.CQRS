using System;
using System.Runtime.Serialization;

namespace Accounts.Application.Exceptions
{
  [Serializable]
  public class VoucherEntryException : Exception
  {
    public VoucherEntryException()
    {
    }

    public VoucherEntryException(string message) : base(message)
    {
    }

    public VoucherEntryException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected VoucherEntryException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
  }
}
