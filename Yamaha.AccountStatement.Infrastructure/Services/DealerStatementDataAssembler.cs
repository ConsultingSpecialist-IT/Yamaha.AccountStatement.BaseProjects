using Yamaha.AccountStatement.Core.Models;
using Yamaha.AccountStatement.Infrastructure.Contracts;

namespace Yamaha.AccountStatement.Infrastructure.Services
{
    public class DealerStatementDataAssembler: IDealerStatementDataAssembler
    {
        private readonly IAccountDealerService _accountDealerService;

        public DealerStatementDataAssembler(IAccountDealerService accountDealerService)
        {
            _accountDealerService = accountDealerService;
        }

        public async Task<StatementPdfData> BuildAsync(AccountDealer dealer, string statementPeriod, CancellationToken cancellationToken)
        {
            var header = await _accountDealerService.GetClientBalanceAsync(dealer.DealerKey, cancellationToken);  //DealerBalance
            var details = await _accountDealerService.GetClientAccountStatementsAsync(dealer.DealerKey, cancellationToken);  //DealerAccountStatement

            decimal balanceDue = details.Data.BalanceDue;

            List<BankAccounts> bankAccounts = [];
            var bankBmx = await _accountDealerService.GetParametersAsync(null, "CUENTA BANAMEX", cancellationToken);
            var bankBBVA = await _accountDealerService.GetParametersAsync(null, "CUENTA BANCOMER", cancellationToken);

            if (bankBmx.IsSuccess)
            {
                bankAccounts.Add(new BankAccounts(
                    bankBmx.Data[0].ParameterName,
                    bankBmx.Data[0].ParameterValue));
            }

            if (bankBBVA.IsSuccess)
            {
                bankAccounts.Add(new BankAccounts(
                    bankBBVA.Data[0].ParameterName,
                    bankBBVA.Data[0].ParameterValue));
            }

            return new StatementPdfData
            {
                Header = header.Data,
                Details = details.Data.Statements,
                BalanceDue = balanceDue,
                BankAccount = bankAccounts
            };
        }
    
    }
}
