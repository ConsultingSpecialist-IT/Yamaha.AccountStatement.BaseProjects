namespace Yamaha.AccountStatement.Core.Models
{
    public sealed record Periods(
        string YearMonthCutOff,
        string YearMonthCutOffDescription);

    public sealed record PeriodsOutPut(
       string AnioMesCorte,
       string AnioMesCorteLetra);
}
