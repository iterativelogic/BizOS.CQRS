using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Common.Application.Contracts;

namespace Infrastructure.Validations
{ 
  [Serializable]
  public class CommandValidationException: Exception
  {
    public CommandValidationException(List<ErrorCode> errorCodes)
    {
      ErrorCodes = errorCodes;
    }

    protected CommandValidationException()
    {
    }

    protected CommandValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public List<ErrorCode> ErrorCodes { get; }
  }
}
