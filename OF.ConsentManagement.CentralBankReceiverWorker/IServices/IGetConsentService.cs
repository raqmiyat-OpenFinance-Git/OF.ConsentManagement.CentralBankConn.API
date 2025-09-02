using OF.ConsentManagement.Model.EFModel;

namespace OF.ConsentManagement.CentralBankReceiverWorker.IServices;
public interface IGetConsentService
{
    Task<long> GetConsentStatusHistoryIdAsync(Guid correlationId, Logger logger);
    Task SaveConsentStatusHistoryAsync(ConsentStatusHistory consentStatusHistory, Logger logger);
    Task<IEnumerable<ConsentIdentifier>> GetConsentRequestIdsAsync(Guid correlationId,
    IEnumerable<string> consentIds, Logger logger);
    Task SaveConsentResponseHistoryAsync(Guid correlationId, List<ConsentResponseHistory> consentResponseHistories, Logger logger);
    Task UpdateConsentStatusHistoryAsync(long id, Guid correlationId, ConsentStatusHistory consentStatusHistory, Logger logger);
}