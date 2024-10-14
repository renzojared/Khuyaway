namespace Khuyaway.EntityFrameworkCore.Options;

public abstract class DbOptions
{
    public virtual string Database { get; set; }
    public string UserId { get; set; }
    public string Password { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
    public int TimeoutInSeconds { get; set; }

    /// <summary>
    /// Default connection string for Sql Server Connections
    /// </summary>
    /// <returns></returns>
    public virtual string DefaultConnectionString() =>
        $"Server={Host},{Port};Database={Database};User Id={UserId};Password={Password};TrustServerCertificate=True;Encrypt=False;";
}