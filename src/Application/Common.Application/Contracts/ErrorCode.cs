namespace Common.Application.Contracts
{
  public class ErrorCode
  {
    public ErrorCode(string code, string description)
    {
      Code = code;
      Description = description;
    }

    public string Code { get; }
    public string Description { get; }

    public static ErrorCode GetRequiredFieldErrorCode(string fieldName) => new ErrorCode($"{fieldName.ToUpper()}_REQUIRED", $"{fieldName} required.");

    public static ErrorCode GetMinimumFieldErrorCode(string fieldName, int length) => new ErrorCode($"{fieldName.ToUpper()}_MIN_LENGTH", $"{fieldName} must be minimum of {length} characters.");

  }
}
