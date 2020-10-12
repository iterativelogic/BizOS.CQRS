
namespace Infrastructure
{
  public static class AppSettings
  {
    public const string MongoDatabaseOptions = "MongoDatabaseOptions";
    public const string DatabaseOptions = "DatabaseOptions";
    public const string SecurityOptions = "SecurityOptions";


    public static class SqlServer
    {
      public const string Port = "1433";
      public const string ConnectionTimeout = "15";
      public const string CommandTimeout = "30";
    }

    public static class Postgres
    {
      public const string Port = "5432";
      public const string ConnectionTimeout = "15";
      public const string CommandTimeout = "30";
    }

    public static class Mongo
    {
      public const string Port = "27017";
      public const string ConnectionTimeout = "30000";
      public const string CommandTimeout = "30000";
    }
  }
}
