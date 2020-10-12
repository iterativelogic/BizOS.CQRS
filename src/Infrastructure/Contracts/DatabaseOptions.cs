namespace Infrastructure.Contracts
{
  public class DatabaseOptions
  {
    public string Server { get; set; }
    public string Database { get; set; }
    public string UserId { get; set; }
    public string Password { get; set; }
    public string Port { get; set; }
    public string CommandTimeout { get; set; }
    public string ConnectionTimeout { get; set; }
  }
}