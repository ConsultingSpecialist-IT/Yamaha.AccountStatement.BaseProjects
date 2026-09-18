namespace Yamaha.AccountStatement.Core.Common
{
    //public sealed record Db2Options(
    // string DatabaseSchema);

    public sealed class Db2Options
    {
        public string DatabaseSchema { get; init; } = string.Empty;
    }
}
