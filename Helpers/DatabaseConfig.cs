using System;

public class DatabaseConfig
{
  public string ConnectionString { get; }

  public DatabaseConfig()
  {
    // Retrieve the database password from the environment variables.
    string dbPassword = Environment.GetEnvironmentVariable("JiTIjugHfVMJzsEo") ?? throw new Exception("DB_PASSWORD environment variable not found.");

    // Use the retrieved password to build the connection string or perform other configurations.
    // For example:
    ConnectionString = $"mongodb+srv://folathecoder:JiTIjugHfVMJzsEo@crowdlaunchcluster.veylgng.mongodb.net/?retryWrites=true&w=majority";
  }
}
