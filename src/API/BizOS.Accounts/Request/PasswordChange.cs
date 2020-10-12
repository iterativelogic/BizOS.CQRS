namespace BizOS.Accounts.Request
{
  public class PasswordChange
  {
    public string OldPassword { get; set; }

    public string NewPassword { get; set; }

    public string ConfirmPassword { get; set; }
  }
}
