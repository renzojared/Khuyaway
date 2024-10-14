namespace Khuyaway.EntityFrameworkCore.Options;

public class OracleDbOptions : DbOptions
{
    public const string SectionKey = nameof(OracleDbOptions);
    public string ServiceName { get; set; }
    public override string Database => UserId;

    public override string DefaultConnectionString() =>
        $"User Id={UserId};Password={Password};Data Source={Host}:{Port}/{ServiceName};";
}