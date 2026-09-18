namespace Yamaha.AccountStatement.Core.Models
{
    public sealed record Parameters(
       int ParameterId,
       string ParameterName,
       string ParameterValue,
       string ParameterDescription,
       short Status);

    public sealed record ParametersOutPut(
       int IdParametro,
       string Nombre,
       string Valor,
       string Descripcion,
       short Estatus);
}
