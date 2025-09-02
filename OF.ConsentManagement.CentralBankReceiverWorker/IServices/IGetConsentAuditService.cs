using OF.ConsentManagement.Model.EFModel;

namespace OF.ConsentManagement.CentralBankReceiverWorker.IServices;
public interface IGetConsentAuditService
{
    Task SaveConsentAuditAsync(ConsentAudit consentAudit, Logger logger);
    Task<long> GetConsentAuditIdAsync(Guid correlationId, Logger logger);
    Task SaveConsentAuditResponseAsync(List<ConsentAuditResponse> consentAuditResponses, Guid correlationId, long consentAuditId, Logger logger);
    Task UpdateConsentAuditAsync(long id, string status, Guid correlationId, Logger logger);
}