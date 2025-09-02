namespace OF.ConsentManagement.CoreBankConn.API.Repositories;
public interface ICbsRepository
{
    Task<string> GetNextSeqAsync(string serviceName, string referenceNbr, Logger logger);
}
