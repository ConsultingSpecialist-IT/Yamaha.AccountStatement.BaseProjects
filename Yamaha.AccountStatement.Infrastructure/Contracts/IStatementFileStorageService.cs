namespace Yamaha.AccountStatement.Infrastructure.Contracts
{
    public interface IStatementFileStorageService
    {
        Task<string> SaveAsync(
            string basePath,
            string period,
            string fileName,
            byte[] content,
            CancellationToken cancellationToken);
    }
}
