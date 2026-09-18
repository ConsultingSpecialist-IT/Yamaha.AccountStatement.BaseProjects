namespace Yamaha.AccountStatement.Infrastructure.Constants
{
    public static class StoredProcedureNames
    {
        public const string GetAccountStatementClients = "SP_GETCLIENTESEDOCTA";  //GetStatement
        public const string GetAccountStatementEnrolledClients = "SP_GETCLIENTESEDOCTA_INSCRITOS";
        public const string InsertUpdateAccountStatementClients = "SP_INSUPDCLIENTESEDOCTA";
        public const string GetClientBalance = "SP_GETSALDOSCLIENTE";  //GetHeader
        public const string GetClientAccountStatement = "SP_GETESTADOCUENTACLIENTE";  //GetDetail
        public const string GetParameters = "SP_GETPARAMETROS";  //GetBankAccount
        public const string GetStatementPeriods = "SP_GETPERIODOS_EDOCTA";

    }
}
