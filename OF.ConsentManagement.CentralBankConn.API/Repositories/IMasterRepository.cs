using OF.ConsentManagement.Model.Common;

namespace OF.ConsentManagement.CentralBankConn.API.Repositories;
public interface IMasterRepository
{
    Task<MasterTableList> GetCachedMasterAsync(Logger logger);
    Task<bool> IsTransactionIdDuplicateAsync(string transactionId, string requestType, Logger logger);
}
