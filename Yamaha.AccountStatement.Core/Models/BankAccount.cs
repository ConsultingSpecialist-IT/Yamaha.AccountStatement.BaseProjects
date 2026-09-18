namespace Yamaha.AccountStatement.Core.Models
{
    public sealed record BankAccount
    (
        string? BanamexAccount,
        string? BancomerAccount
    );

    public sealed record BankAccounts
    (
        string? AccountName,
        string? AccountNumber
    );
}
