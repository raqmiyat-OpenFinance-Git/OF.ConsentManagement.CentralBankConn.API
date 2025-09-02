using OF.ConsentManagement.Model.EFModel;

namespace OF.ConsentManagement.CentralBankReceiverWorker.IServices;
public interface IPatchConsentService
{
    Task<long> GetConsentRequestIdAsync(string consentId, Logger logger);
    Task<bool> UpdateConsentRequestAsync(long id, string consentStatus, Logger logger);
    Task<bool> UpdateConsentResponseAsync(long id, ConsentResponse consentResponse, Logger logger);
}