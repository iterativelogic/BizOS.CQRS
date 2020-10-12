
namespace Infrastructure.Models
{
  public class Message
  {
    public Message(MessageType type, string code, string messageText)
    {
      Type = type.ToString();
      Code = code;
      Text = messageText;
    }
    public readonly string Type;
    public readonly string Code;
    public readonly string Text;
  }
}
