using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Yamaha.AccountStatement.Core.Common;
using Yamaha.AccountStatement.Core.Models;
using Yamaha.AccountStatement.Infrastructure.Contracts;
using static Yamaha.AccountStatement.Infrastructure.Contracts.IDealerStatementDataAssembler;

namespace Yamaha.AccountStatement.Infrastructure.Services
{
    public class DealerStatementGenerationService : IDealerStatementGenerationService
    {
        private readonly ILogger _logger;
        private readonly IAccountDealerService _accountDealerService;
        private readonly IStatementFileNameBuilder _statementFileNameBuilder;
        private readonly IDealerStatementDataAssembler _dealerStatementDataAssembler;
        private readonly IStatementPdfGenerator _statementPdfGenerator;
        private readonly IStatementFileStorageService _statementFileStorageService;
        private readonly string _filesBasePath;

        public DealerStatementGenerationService(ILogger<DealerStatementGenerationService> logger, 
                IAccountDealerService accountDealerService, IStatementFileNameBuilder statementFileNameBuilder,
                IDealerStatementDataAssembler dealerStatementDataAssembler, IStatementPdfGenerator statementPdfGenerator, 
                IStatementFileStorageService statementFileStorageService, IConfiguration configuration)
        {
            _accountDealerService = accountDealerService;
            _statementFileNameBuilder = statementFileNameBuilder;
            _dealerStatementDataAssembler = dealerStatementDataAssembler;
            _logger = logger;
            _statementPdfGenerator = statementPdfGenerator;
            _statementFileStorageService = statementFileStorageService;
            _filesBasePath = configuration["FilesBasePath"]
                ?? throw new InvalidOperationException("No se encontró la configuración 'FilesBasePath'.");
        }

        public async Task ProcessStatementsAsync(CancellationToken cancellationToken) 
        {
            _logger.LogInformation("Inicio del proceso de generación de estados de cuenta.");

            string statementPeriod = _statementFileNameBuilder.GetStatementPeriod();

            var dealersResponse = await _accountDealerService.GetAccountDealers000Async(cancellationToken);

            if (dealersResponse?.Data == null || dealersResponse.Data.Count == 0)
            {
                _logger.LogWarning("No se encontraron distribuidores para procesar.");
                return;
            }

            foreach (var dealer in dealersResponse.Data)
            {
                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    var pdfData = await _dealerStatementDataAssembler.BuildAsync(dealer, statementPeriod, cancellationToken);
                    byte[] pdfBytes = _statementPdfGenerator.Generate(pdfData);
                    string fileName = _statementFileNameBuilder.BuildFileName(dealer.DealerKey, dealer.BillingSuffix,statementPeriod);

                    string fullPath = await _statementFileStorageService.SaveAsync(
                        _filesBasePath,
                        statementPeriod,
                        fileName,
                        pdfBytes,
                        cancellationToken);

                    _logger.LogInformation(
                        "Estado de cuenta creado correctamente en {FullPath}.",
                        fullPath);
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Error procesando estado de cuenta para DealerKey={DealerKey}, DealerLocationId={DealerLocationId}.",
                        dealer.DealerKey,
                        dealer.BillingSuffix);
                }

            }

            _logger.LogInformation("Fin del proceso de generación de estados de cuenta.");
        }

        public async Task<string> ProcessStatementAsync(string dealerKey, string billingSuffix, string statementPeriod, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _logger.LogInformation("Inicio de generación de estado de cuenta. DealerKey={DealerKey}, BillingSuffix={BillingSuffix}, Period={Period}", dealerKey, billingSuffix, statementPeriod);
            
            try
            {
                // Obtener información completa del dealer
                var dealersResponse = await _accountDealerService.GetAccountDealersAsync(dealerKey, cancellationToken);

                if (dealersResponse?.Data == null || dealersResponse.Data.Count == 0)
                {
                    throw new InvalidOperationException($"No se encontró el dealer {dealerKey}.");
                }

                // Buscar exactamente DealerKey + BillingSuffix
                var dealer = dealersResponse.Data.FirstOrDefault(x => string.Equals(x.DealerKey, dealerKey, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(x.BillingSuffix, billingSuffix, StringComparison.OrdinalIgnoreCase));

                if (dealer == null)
                {
                    throw new InvalidOperationException($"No se encontró el dealer {dealerKey} con BillingSuffix {billingSuffix}.");
                }


                var pdfData = await _dealerStatementDataAssembler.BuildAsync(dealer, statementPeriod, cancellationToken);
                byte[] pdfBytes = _statementPdfGenerator.Generate(pdfData);
                string fileName = _statementFileNameBuilder.BuildFileName(dealer.DealerKey, dealer.BillingSuffix, statementPeriod);

                string fullPath = await _statementFileStorageService.SaveAsync(
                    _filesBasePath,
                    statementPeriod,
                    fileName,
                    pdfBytes,
                    cancellationToken);

                _logger.LogInformation("Estado de cuenta creado correctamente en {FullPath}.", fullPath);
                
                return fullPath;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error procesando estado de cuenta para DealerKey={DealerKey}, BillingSuffix={BillingSuffix}, Period={Period}.",
                    dealerKey,
                    billingSuffix,
                    statementPeriod);

                throw;
            }
        }

        private static string GetStatementPeriod()
        {
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)
                .AddMonths(-1)
                .ToString("yyyy-MM");
        }
    }

}
