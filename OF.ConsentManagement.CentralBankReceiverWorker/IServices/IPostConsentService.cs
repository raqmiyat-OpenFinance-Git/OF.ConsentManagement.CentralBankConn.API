using OF.ConsentManagement.Model.EFModel;

namespace OF.ConsentManagement.CentralBankReceiverWorker.IServices;
public interface IPostConsentService
{
    Task SaveConsentRequestAsync(ConsentRequest consentRequest, Logger logger);
    Task<long> GetConsentRequestIdAsync(Guid correlationId, Logger logger);
    Task SaveConsentResponseAsync(long id, Guid correlationId, ConsentResponse consentResponse, Logger logger);
    Task<bool> UpdateConsentRequestStatusAsync(long id, string status, Guid correlationId, Logger logger);
}