using Yamaha.AccountStatement.Core.Models;
using Yamaha.AccountStatement.Core.Request;
using Yamaha.AccountStatement.Core.Response;

namespace Yamaha.AccountStatement.Infrastructure.Contracts
{
    public interface IAccountDealerService
    {
        Task<BaseDataResponse<List<AccountDealer>>> GetAccountDealersAsync(CancellationToken cancellationToken);
        Task<BaseDataResponse<List<AccountDealer>>> GetAccountDealersAsync(string? dealerKey, CancellationToken cancellationToken);
        Task<BaseDataResponse<List<AccountDealer>>> GetEnrolledAccountDealersAsync(string? dealerKey, CancellationToken cancellationToken);
        Task<BaseDataResponse<FileData>> GetAccountStatementFileAsync(string clientKey, string branchCode, string month, int year, CancellationToken cancellationToken);
        Task<BaseDataResponse<List<AccountDealer>>> UpdateAccountDealerStatusAsync(List<UpdateAccountDealerStatusRequest> request, CancellationToken cancellationToken);
        Task<BaseDataResponse<DealerBalance>> GetClientBalanceAsync(string clientKey, CancellationToken cancellationToken);
        Task<BaseDataResponse<List<DealerAccountStatement>>> GetClientAccountStatementAsync(string clientKey, CancellationToken cancellationToken);
        Task<BaseDataResponse<DealerAccountStatementResult>> GetClientAccountStatementsAsync(string clientKey, CancellationToken cancellationToken);
        Task<BaseDataResponse<List<Periods>>> GetPeriodsAccountStatementAsync(string period, CancellationToken cancellationToken);
        Task<BaseDataResponse<List<Parameters>>> GetParametersAsync(int? idParameter, string parameterName, CancellationToken cancellationToken);
        /*Task<DealerStatementHeader> GetStatementHeaderAsync(HeaderQuery request, CancellationToken cancellationToken);
        Task<List<DealerInvoiceDetail>> GetInvoiceDetailsAsync(HeaderQuery request, CancellationToken cancellationToken);
        Task<double> GetBalanceDueAsync(HeaderQuery request, CancellationToken cancellationToken);
        Task<BankAccount> GetBankAccountAsync(CancellationToken cancellationToken);*/
    }
}
